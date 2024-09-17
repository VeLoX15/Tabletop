using Blazor.Pagination;
using DbController;
using DbController.SqlServer;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Games
{
    public partial class GameHistory : IHasPagination
    {
        private int _page = 1;
        private User? _loggedInUser;
        public GameFilter Filter { get; set; } = new();

        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }

        public bool OpenFilter { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            _loggedInUser = await authService.GetUserAsync();

            if (_loggedInUser != null)
            {
                Filter = new()
                {
                    Limit = AppdataService.PageLimit,
                    UserId = _loggedInUser.UserId
                };

                using IDbController dbController = new SqlController(AppdataService.ConnectionString);

                List<User> friends = await UserService.GetUserFriendsAsync(_loggedInUser.Id, dbController);
            }

            OpenFilter = false;

            await LoadAsync();
        }

        public async Task LoadAsync(bool navigateToPage1 = false)
        {
            Filter.PageNumber = navigateToPage1 ? 1 : Page;
            using IDbController dbController = new SqlController(AppdataService.ConnectionString);
            TotalItems = await Service.GetTotalAsync(Filter, dbController);
            Data = await Service.GetAsync(Filter, dbController);
        }

        protected override async Task DeleteAsync()
        {
            await base.DeleteAsync();
            await LoadAsync();
        }
    }
}