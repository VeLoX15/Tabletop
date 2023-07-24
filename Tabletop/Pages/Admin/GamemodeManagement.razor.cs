using Blazor.Pagination;
using DbController;
using DbController.MySql;
using Tabletop.Core.Filters;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Admin
{
    public partial class GamemodeManagement : IHasPagination
    {
        private int _page = 1;
        public GamemodeFilter Filter { get; set; } = new()
        {
            Limit = AppdataService.PageLimit
        };
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            await LoadAsync();
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
            Filter.PageNumber = navigateToPage1 ? 1 : Page;
            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
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
            Input = new();

            return Task.CompletedTask;
        }

        protected async Task UpdateAppdata()
        {
            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
            AppdataService.Units = await UnitService.GetAllAsync(dbController);
            await JSRuntime.ShowToastAsync(ToastType.success, "App data for game modes reloaded");
        }
    }
}