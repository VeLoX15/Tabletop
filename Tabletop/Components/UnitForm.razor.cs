using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Tabletop.Models;

namespace Tabletop.Components
{
    public partial class UnitForm
    {
        [Parameter] 
        public Unit Input { get; set; } = new();
        [Parameter]
        public EventCallback<Unit> OnSaved { get; set; }

        private EditForm? _form;

        public async Task SaveUnit()
        {
            if (_form == null || _form.EditContext is null) 
            {
                return;
            }

            if (_form.EditContext.Validate())
            {
                await OnSaved.InvokeAsync(Input);
            }
        }
        
    }
}