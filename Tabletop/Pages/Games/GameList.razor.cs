using Blazor.Pagination;
using DbController;
using DbController.SqlServer;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Games
{
    public partial class GameList : IHasPagination
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

        protected override async Task SaveAsync()
        {
            if (_form is null || _form.EditContext is null || Input is null)
            {
                return;
            }

            if (_form.EditContext.Validate())
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                await dbController.StartTransactionAsync();
                try
                {
                    if (Input.Id is 0)
                    {
                        await Service.CreateAsync(Input, dbController);
                    }
                    else
                    {
                        await Service.UpdateAsync(Input, dbController);
                        navigationManager.NavigateTo($"/Account/Games/{Input.GameId}");
                    }

                    await dbController.CommitChangesAsync();
                    AppdataService.UpdateRecord(Input);
                }
                catch (Exception)
                {
                    await dbController.RollbackChangesAsync();
                    throw;
                }

                await JSRuntime.ShowToastAsync(ToastType.success, $"Save item");
                Input = null;
                await LoadAsync();
            }
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
    }
}