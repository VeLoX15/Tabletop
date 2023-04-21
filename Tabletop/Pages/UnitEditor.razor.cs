using DbController;
using Tabletop.Services;
using Tabletop.Models;
using Microsoft.AspNetCore.Components;

namespace Tabletop.Pages
{
    public partial class UnitEditor
    {
        [Parameter]
        public int UnitId { get; set; }
        public Unit Input { get; set; } = new();
        public Unit StartCopy { get; set; } = new();
        public List<Weapon> WeaponList { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {

            if (UnitId > 0)
            {
                await Task.Run(LoadEditModeAsync);
            }
            else
            {
                Input = new Unit();
            }
        }

        public async Task LoadEditModeAsync()
        {
            using IDbController dbController = dbProviderService.GetDbController(AppdatenService.ConnectionString);

            Unit? form = await unitService.GetAsync(UnitId, dbController);

            if (form is not null)
            {
                //Input = form.DeepCopyByExpressionTree();
                //StartCopy = form.DeepCopyByExpressionTree();
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
                if (Input.UnitId is 0)
                {
                    await unitService.CreateAsync(Input, dbController);

                }
                else
                {
                    await unitService.UpdateAsync(Input, dbController);

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
                navigationManager.NavigateTo($"/Admin/FormEditor/{Input.UnitId}");
            }
            else
            {
                await OnParametersSetAsync();

            }

            await jsRuntime.ShowToastAsync(ToastType.success, "Unit has been saved successfully.");

        }
    }
}
