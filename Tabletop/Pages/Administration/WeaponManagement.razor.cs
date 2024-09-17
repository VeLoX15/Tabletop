using Blazor.Pagination;
using DbController;
using DbController.SqlServer;
using Tabletop.Core.Filters;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Administration
{
    public partial class WeaponManagement : IHasPagination
    {
        private int _page = 1;
        public WeaponFilter Filter { get; set; } = new()
        {
            Limit = AppdataService.PageLimit
        };
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }
        public bool OpenFilter { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            OpenFilter = false;

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
            Input = new();

            return Task.CompletedTask;
        }

        protected async Task UpdateAppdata()
        {
            using IDbController dbController = new SqlController(AppdataService.ConnectionString);
            AppdataService.Weapons = await WeaponService.GetAllAsync(dbController);
            await JSRuntime.ShowToastAsync(ToastType.success, localizer["WEAPONS"] + " " + localizer["RELOADED"]);
        }
    }
}