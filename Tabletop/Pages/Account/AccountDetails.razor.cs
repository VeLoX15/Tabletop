using DbController.MySql;
using DbController;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Tabletop.Core.Models;
using Tabletop.Core.Services;
using Blazor.Pagination;
using Tabletop.Core.Filters;
using Microsoft.JSInterop;

namespace Tabletop.Pages.Account
{
    public partial class AccountDetails : IHasPagination
    {
#nullable disable
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
#nullable enable

        public User? CurrentUser { get; set; }
        private ClaimsPrincipal? _user;
        protected override async Task OnParametersSetAsync()
        {
            if (AuthState is not null)
            {
                _user = (await AuthState).User;

                CurrentUser = await authService.GetUserAsync();

                if (CurrentUser?.Image != null)
                {
                    string base64String = Convert.ToBase64String(CurrentUser.Image);
                    CurrentUser.ConvertedImage = $"data:image/png;base64,{base64String}";
                }

                await LoadContent();
                await SelectUnits();

                _loggedInUser = await authService.GetUserAsync();
                await LoadAsync();
            }
        }

        public int Option { get; set; }
        public int SelectedFraction { get; set; }
        public List<Fraction> Fractions { get; set; } = new(); 
        public List<Unit> UserUnits { get; set; } = new();
        public List<Unit> Units { get; set; } = new();
        public Unit Unit { get; set; } = new();
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }
        private int _page = 1;
        private User? _loggedInUser;
        public UserFilter Filter { get; set; } = new()
        {
            Limit = AppdataService.PageLimit
        };
        public Permission? SelectedPermission { get; set; }
        public bool IsSearching { get; set; } = false;
        public List<User> Users { get; set; } = new();
        public List<User> Friends { get; set; } = new();

        public async Task LoadAsync(bool navigateToPage1 = false)
        {
            Filter.PageNumber = navigateToPage1 ? 1 : Page;
            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
            TotalItems = await userService.GetTotalAsync(Filter, dbController);
            Users = await userService.GetAsync(Filter, dbController);
        }

        protected Task HandleClick()
        {
            IsSearching = true;
            return Task.CompletedTask;
        }

        protected Task Menu(int option)
        {
            Option = option;
            return Task.CompletedTask;
        }

        protected async Task LoadContent()
        {
            if (CurrentUser != null)
            {
                Fractions = AppdataService.Fractions.ToList();

                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                UserUnits = await unitservice.GetUserUnitsAsync(CurrentUser.Id, dbController);

                Friends = await userService.GetUserFriendsAsync(CurrentUser.Id, dbController);

                foreach(User item in Friends)
                {
                    if (item?.Image != null)
                    {
                        string base64String = Convert.ToBase64String(item.Image);
                        item.ConvertedImage = $"data:image/png;base64,{base64String}";
                    }
                }
            }
        }

        protected async Task SelectUnits()
        {
            Units = UserUnits.Where(x => x.FractionId == SelectedFraction).ToList();
            await LoadContent();
        }

        protected async Task AddFriend(int friendId)
        {
            if(CurrentUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                await userService.CreateUserFriendAsync(CurrentUser.Id, friendId, dbController);
            }
        }


#nullable disable
        [Inject] public IJSRuntime JSRuntime { get; set; }
#nullable enable

        protected virtual async Task DeleteAsync(int friendId)
        {
            if (CurrentUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                await userService.DeleteFriendAsync(CurrentUser.Id, friendId, dbController);
                await JSRuntime.ShowToastAsync(ToastType.success, "Delete Massage");
            }
        }
    }
}