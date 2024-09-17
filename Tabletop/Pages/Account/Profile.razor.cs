using DbController.SqlServer;
using DbController;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Tabletop.Core.Models;
using Tabletop.Core.Services;
using Blazor.Pagination;
using Tabletop.Core.Filters;
using Microsoft.JSInterop;
using Tabletop.Core.Calculators;
using System.Globalization;
using Tabletop.Core;

namespace Tabletop.Pages.Account
{
    public partial class Profile : IHasPagination
    {
#nullable disable
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
#nullable enable

        [Parameter]
        public string UserName { get; set; } = string.Empty;
        public User? CurrentUser { get; set; }

        private User? _loggedInUser;

        public int SelectedFraction { get; set; } = 1;
        public List<Fraction> Fractions { get; set; } = [];
        public List<Unit> UserUnits { get; set; } = [];
        public List<Unit> Units { get; set; } = [];
        public Unit Unit { get; set; } = new();
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }

        private int _page = 1;
        public bool AddFriendModal { get; set; }
        public bool AddUnitModal { get; set; }
        public bool EditProfile { get; set; }
        public bool IsFriend { get; set; }
        public List<User> Users { get; set; } = [];
        public List<User> Friends { get; set; } = [];
        public UserFilter Filter { get; set; } = new()
        {
            Limit = AppdataService.PageLimit
        };

        protected override async Task OnParametersSetAsync()
        {
            if (AuthState != null)
            {
                _loggedInUser = await authService.GetUserAsync();

                UserName ??= _loggedInUser?.Username ?? string.Empty;

                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                CurrentUser = await UserService.GetAsync(UserName, dbController);

                if (CurrentUser != null)
                {
                    SelectedFraction = CurrentUser.MainFractionId;
                }

                if (CurrentUser?.Image != null)
                {
                    string base64String = Convert.ToBase64String(CurrentUser.Image);
                    CurrentUser.ConvertedImage = $"data:image/png;base64,{base64String}";
                }

                AddFriendModal = false;
                AddUnitModal = false;
                EditProfile = false;       

                await LoadContent();
                await SelectFractionUnitsByUser();

                await LoadAsync();
                await CalculateForceAsync();
                await CheckFriendAsync();
            }
        }

        public async Task CheckFriendAsync()
        {
            if (CurrentUser != null && _loggedInUser != null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                IsFriend = await UserService.CheckUserFriendAsync(_loggedInUser.Id, CurrentUser.Id, dbController);
            }
        }

        private async Task CalculateForceAsync()
        {
            if (CurrentUser is not null)
            {
                foreach (var unit in UserUnits)
                {
                    int force = await Calculation.ForceAsync(unit);

                    unit.Force = force;
                    unit.ForceOfQuantity = force * unit.Quantity;
                }
            }
        }

        public async Task LoadAsync(bool navigateToPage1 = false)
        {
            Filter.PageNumber = navigateToPage1 ? 1 : Page;
            if (Filter.SearchPhrase != string.Empty)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                TotalItems = await userService.GetTotalAsync(Filter, dbController);
                Users = await userService.GetAsync(Filter, dbController);
            }
        }

        protected async Task LoadContent()
        {
            if (CurrentUser != null)
            {
                Fractions = [.. AppdataService.Fractions];
                Units = [.. AppdataService.Units];

                await FriendReloading();
                await UnitReloading();
            }
        }

        protected async Task FriendReloading()
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);

                Friends = await UserService.GetUserFriendsAsync(CurrentUser.Id, dbController);

                foreach (User item in Friends)
                {
                    if (item.Image != null)
                    {
                        string base64String = Convert.ToBase64String(item.Image);
                        item.ConvertedImage = $"data:image/png;base64,{base64String}";
                    }
                }
            }
        }

        protected async Task UnitReloading()
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                UserUnits = await UnitService.GetUserUnitsAsync(CurrentUser.Id, dbController);
            }
        }

        protected async Task SelectFractionUnitsByUser()
        {
            await LoadContent();
        }

        protected async Task AddFriendAsync(int friendId)
        {
            if (_loggedInUser != null && CurrentUser != null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                await UserService.CreateUserFriendAsync(_loggedInUser.Id, friendId, dbController);
                await FriendReloading();
                await JSRuntime.ShowToastAsync(ToastType.success, CurrentUser.Username + " " + localizer["FOLLOWED"]);
            }
        }

        protected async Task AddUnitAsync(Unit unit)
        {
            if (CurrentUser is not null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);

                CurrentUser.Units.Add(unit);
                await UnitService.CreateUserUnitAsync(CurrentUser, unit, dbController);
                await UnitReloading();
                await JSRuntime.ShowToastAsync(ToastType.success, unit.GetLocalization(CultureInfo.CurrentCulture)?.Name + " " + localizer["ADDED"]);
            }
        }

        protected async Task EditProfileAsync()
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                await userService.UpdateAsync(CurrentUser, dbController);
                await JSRuntime.ShowToastAsync(ToastType.success, localizer["PROFILE"] + " " + localizer["EDITED"]);
            }
        }

        protected async Task DeleteFriendAsync()
        {
            if (CurrentUser != null && _loggedInUser != null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                await UserService.DeleteFriendAsync(_loggedInUser.Id, CurrentUser.Id, dbController);
                await FriendReloading();
                await JSRuntime.ShowToastAsync(ToastType.success, CurrentUser.Username + " " + localizer["UNFOLLOWED"]);
            }
        }

        protected async Task DeleteUnitAsync(Unit unit)
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                await unitService.DeleteUnitAsync(CurrentUser.Id, unit.UnitId, dbController);
                await UnitReloading();
                await JSRuntime.ShowToastAsync(ToastType.success, unit.GetLocalization(CultureInfo.CurrentCulture)?.Name + " " + localizer["DELETED"]);
            }
        }
    }
}