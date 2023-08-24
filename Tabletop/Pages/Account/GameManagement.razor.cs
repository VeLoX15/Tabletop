using Blazor.Pagination;
using DbController;
using DbController.MySql;
using Microsoft.AspNetCore.Components;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Account
{
    public partial class GameManagement : IHasPagination
    {
        private int _page = 1;
        private User? _loggedInUser;
        public GameFilter GameFilter { get; set; } = new()
        {
            Limit = AppdataService.PageLimit
        };

        public Player? SelectedUser { get; set; }
        public List<Player> Friends { get; set; } = new();
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            _loggedInUser = await authService.GetUserAsync();

            if (_loggedInUser != null)
            {
                GameFilter = new()
                {
                    Limit = AppdataService.PageLimit,
                    UserId = _loggedInUser.UserId
                };
            }

            await FriendReloading();
            await LoadAsync();
        }

        protected async Task FriendReloading()
        {
            if (_loggedInUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);

                List<User> friends = await userService.GetUserFriendsAsync(_loggedInUser.Id, dbController);

                foreach (var user in friends)
                {
                    Friends.Add(new Player()
                    {
                        User = user
                    });
                }
            }
        }

        protected override async Task SaveAsync()
        {
            if (Input is null)
            {
                return;
            }

            await base.SaveAsync();
            await LoadAsync();
        }

        public async Task LoadAsync(bool navigateToPage1 = false)
        {
            GameFilter.PageNumber = navigateToPage1 ? 1 : Page;
            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
            TotalItems = await Service.GetTotalAsync(GameFilter, dbController);
            Data = await Service.GetAsync(GameFilter, dbController);
        }

        protected override async Task DeleteAsync()
        {
            await base.DeleteAsync();
            await LoadAsync();
        }

        protected override Task NewAsync()
        {
            if (_loggedInUser != null)
            {
                Input = new Game
                {
                    UserId = _loggedInUser.UserId
                };
            }

            return Task.CompletedTask;
        }

        private Task AddUserAsync(int team)
        {
            if (Input is not null)
            {
                if (SelectedUser is not null)
                {
                    Input.Players.Add(new Player()
                    {
                        UserId = SelectedUser.UserId,
                        GameId = Input.GameId,
                        Team = team
                    });
                }
                SelectedUser = null;
            }

            return Task.CompletedTask;
        }

        private Task UserSelectionChangedAsync(ChangeEventArgs e)
        {
            int userId = Convert.ToInt32(e.Value);
            SelectedUser = Friends.FirstOrDefault(x => x.User.UserId == userId);
            return Task.CompletedTask;
        }
    }
}