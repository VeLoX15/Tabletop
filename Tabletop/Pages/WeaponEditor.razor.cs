using DbController;
using Tabletop.Services;
using Tabletop.Models;
using Microsoft.AspNetCore.Components;
using Tabletop.Validator;

namespace Tabletop.Pages
{
    public partial class WeaponEditor
    {
        [Parameter]
        public int UnitId { get; set; }
        public Weapon Input { get; set; } = new();
        private WeaponValidator Validator { get; set; } = new WeaponValidator();

        protected override async Task OnParametersSetAsync()
        {

            if (UnitId > 0)
            {
                await Task.Run(LoadEditModeAsync);
            }
            else
            {
                Input = new Weapon();
            }
        }

        public async Task LoadEditModeAsync()
        {
            using IDbController dbController = dbProviderService.GetDbController(AppdatenService.ConnectionString);

            Weapon? unit = await weaponService.GetAsync(UnitId, dbController);

            if (unit is not null)
            {
                Input = unit;
            }
        }

        public async Task SaveAsync()
        {
            if (Input is null)
            {
                return;
            }


            using IDbController dbController = dbProviderService.GetDbController(AppdatenService.ConnectionString);

            await dbController.StartTransactionAsync();

            try
            {
                if (Input.WeaponId is 0)
                {
                    await weaponService.CreateAsync(Input, dbController);

                }
                else
                {
                    await weaponService.UpdateAsync(Input, dbController);

                }

                await dbController.CommitChangesAsync();
            }
            catch (Exception)
            {
                await dbController.RollbackChangesAsync();
                throw;
            }

            if (UnitId is 0)
            {
                navigationManager.NavigateTo($"/Weapon/{Input.WeaponId}");
            }
            else
            {
                await OnParametersSetAsync();

            }

            await jsRuntime.ShowToastAsync(ToastType.success, "Weapon has been saved successfully.");

        }

        private Task CloseItemAsync()
        {
            navigationManager.NavigateTo("/Weapons");
            return Task.CompletedTask;
        }
    }
}
