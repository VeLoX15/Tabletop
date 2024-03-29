using DbController.MySql;
using DbController;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Tabletop.Core.Models;
using Tabletop.Core.Services;
using Blazor.Pagination;
using Tabletop.Core.Filters;
using Microsoft.JSInterop;

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
        public List<Fraction> Fractions { get; set; } = new();
        public List<Unit> UserUnits { get; set; } = new();
        public List<Unit> Units { get; set; } = new();
        public Unit Unit { get; set; } = new();
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }

        private int _page = 1;
        public bool AddFriendModal { get; set; } = false;
        public bool AddUnitModal { get; set; } = false;
        public bool EditProfile { get; set; } = false;
        public List<User> Users { get; set; } = new();
        public List<User> Friends { get; set; } = new();
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

                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                CurrentUser = await userService.GetAsync(UserName, dbController);

                if (CurrentUser?.MainFractionId > 0)
                {
                    SelectedFraction = CurrentUser.MainFractionId;
                }

                if (CurrentUser?.Image != null)
                {
                    string base64String = Convert.ToBase64String(CurrentUser.Image);
                    CurrentUser.ConvertedImage = $"data:image/png;base64,{base64String}";
                }

                await LoadContent();
                await SelectFractionUnitsByUser();

                await LoadAsync();
            }
        }

        public async Task LoadAsync(bool navigateToPage1 = false)
        {
            Filter.PageNumber = navigateToPage1 ? 1 : Page;
            if (Filter.SearchPhrase != string.Empty)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                TotalItems = await userService.GetTotalAsync(Filter, dbController);
                Users = await userService.GetAsync(Filter, dbController);
            }
        }

        protected Task OpenFriendModal()
        {
            AddFriendModal = true;
            Filter.SearchPhrase = string.Empty;
            Users = new();
            return Task.CompletedTask;
        }

        protected Task OpenUnitModal()
        {
            AddUnitModal = true;
            return Task.CompletedTask;
        }

        protected Task OpenEditProfile()
        {
            EditProfile = true;
            return Task.CompletedTask;
        }

        protected async Task LoadContent()
        {
            if (CurrentUser != null)
            {
                Fractions = AppdataService.Fractions.ToList();
                Units = AppdataService.Units.ToList();

                await FriendReloading();
                await UnitReloading();
            }
        }

        protected async Task FriendReloading()
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);

                Friends = await userService.GetUserFriendsAsync(CurrentUser.Id, dbController);

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
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                UserUnits = await unitService.GetUserUnitsAsync(CurrentUser.Id, dbController);
            }
        }

        protected async Task SelectFractionUnitsByUser()
        {
            await LoadContent();
        }

        protected async Task AddFriendAsync(int friendId)
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                await userService.CreateUserFriendAsync(CurrentUser.Id, friendId, dbController);
                await FriendReloading();
                await JSRuntime.ShowToastAsync(ToastType.success, "Friend has been added");
            }
        }

        protected async Task AddUnitAsync(Unit unit)
        {
            if (CurrentUser is not null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);

                CurrentUser.Units.Add(unit);
                await unitService.CreateUserUnitAsync(CurrentUser, unit, dbController);
                await UnitReloading();
                await JSRuntime.ShowToastAsync(ToastType.success, "Unit has been added");
            }
        }

        protected async Task EditProfileAsync()
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                await userService.UpdateAsync(CurrentUser, dbController);
                await JSRuntime.ShowToastAsync(ToastType.success, "Profile has been edited");
            }
        }

        protected async Task DeleteFriendAsync(int friendId)
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                await userService.DeleteFriendAsync(CurrentUser.Id, friendId, dbController);
                await FriendReloading();
                await JSRuntime.ShowToastAsync(ToastType.success, "Friend has been deleted");
            }
        }

        protected async Task DeleteUnitAsync(int unitId)
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                await unitService.DeleteUnitAsync(CurrentUser.Id, unitId, dbController);
                await UnitReloading();
                await JSRuntime.ShowToastAsync(ToastType.success, "Unit has been deleted");
            }
        }
    }
}