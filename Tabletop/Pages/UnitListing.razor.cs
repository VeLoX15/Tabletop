using Blazor.Pagination;
using DbController;
using Microsoft.AspNetCore.Components;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages
{
    public partial class UnitListing : IHasPagination
    {
        public UnitFilter Filter { get; set; } = new()
        {
            Limit = AppdatenService.PageLimit
        };

        public List<Unit> Data { get; set; } = new();
        [Parameter]
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public Unit? SelectedForDeletion { get; set; }

        public async Task LoadAsync(bool navigateToPage1 = false)
        {
            if (navigateToPage1)
            {
                navigationManager.NavigateTo("/Units/");
            }

            Filter.PageNumber = Page;
            using IDbController dbController = dbProviderService.GetDbController(AppdatenService.ConnectionString);
            TotalItems = await unitService.GetTotalAsync(Filter, dbController);
            Data = await unitService.GetAsync(Filter, dbController);
        }

        private async Task DeleteAsync()
        {
            if (SelectedForDeletion is null)
            {
                return;
            }

            using IDbController dbController = dbProviderService.GetDbController(AppdatenService.ConnectionString);

            await dbController.StartTransactionAsync();

            try
            {
                await unitService.DeleteAsync(SelectedForDeletion, dbController);
                await dbController.CommitChangesAsync();
                await jsRuntime.ShowToastAsync(ToastType.success, "Unit was successfully deleted.");
                SelectedForDeletion = null;
            }
            catch (Exception)
            {
                await dbController.RollbackChangesAsync();
                throw;
            }

            await LoadAsync();

        }

    }
}