using DbController;
using DbController.MySql;
using Tabletop.Core;
using Tabletop.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Tabletop.Pages.Admin
{
    public abstract class ManagementBasePage<T, TService> : ComponentBase where T : class, IDbModel, new() where TService : IModelService<T>
    {
        protected T? Input { get; set; }
        protected T? SelectedForDeletion { get; set; }
        protected EditForm? _form;
#nullable disable
        [Inject] public TService Service { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
#nullable enable

        protected List<T> Data { get; set; } = new();

        protected override Task OnInitializedAsync()
        {
            Data = AppdataService.GetList<T>();
            return base.OnInitializedAsync();
        }
        protected virtual Task NewAsync()
        {
            Input = new T();
            return Task.CompletedTask;
        }

        protected virtual Task SelectForDeletionAsync(T input)
        {
            SelectedForDeletion = input;
            return Task.CompletedTask;
        }

        protected virtual Task EditAsync(T input)
        {
            Input = input.DeepCopyByExpressionTree();
            return Task.CompletedTask;
        }

        protected virtual async Task DeleteAsync()
        {
            if (SelectedForDeletion is null)
            {
                return;
            }
            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
            await dbController.StartTransactionAsync();

            try
            {
                await Service.DeleteAsync(SelectedForDeletion, dbController);
                await dbController.CommitChangesAsync();
                AppdataService.DeleteRecord(SelectedForDeletion);
                await JSRuntime.ShowToastAsync(ToastType.success, "Delete item");
                SelectedForDeletion = null;
            }
            catch (Exception ex)
            {
                await dbController.RollbackChangesAsync();
                if (ex.HResult == -2147467259)
                {
                    await JSRuntime.ShowToastAsync(ToastType.error, "Delete error");
                }
                else
                {
                    throw;
                }
            }
        }

        protected virtual async Task SaveAsync()
        {
            if (_form is null || _form.EditContext is null || Input is null)
            {
                return;
            }

            if (_form.EditContext.Validate())
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
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
            }
        }
    }
}
