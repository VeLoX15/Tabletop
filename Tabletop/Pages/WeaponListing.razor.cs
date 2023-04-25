using Blazor.Pagination;
using DbController;
using Microsoft.AspNetCore.Components;
using Tabletop.Filters;
using Tabletop.Models;
using Tabletop.Services;

namespace Tabletop.Pages
{
    public partial class WeaponListing : IHasPagination
    {
        public WeaponFilter Filter { get; set; } = new()
        {
            Limit = AppdatenService.PageLimit
        };

        public List<Weapon> Data { get; set; } = new();
        [Parameter]
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public Weapon? SelectedForDeletion { get; set; }

        public async Task LoadAsync(bool navigateToPage1 = false)
        {
            if (navigateToPage1)
            {
                navigationManager.NavigateTo("/Weapons/");
            }

            Filter.PageNumber = Page;
            using IDbController dbController = dbProviderService.GetDbController(AppdatenService.ConnectionString);
            TotalItems = await weaponService.GetTotalAsync(Filter, dbController);
            Data = await weaponService.GetAsync(Filter, dbController);
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
                await weaponService.DeleteAsync(SelectedForDeletion, dbController);
                await dbController.CommitChangesAsync();
                await jsRuntime.ShowToastAsync(ToastType.success, "Weapon was successfully deleted.");
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