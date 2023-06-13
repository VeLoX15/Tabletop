using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;

namespace Tabletop.Components.Cards
{
    public partial class UnitOverview
    {
        [Parameter]
        public List<Unit> Units { get; set; } = new();
        [Parameter]
        public EventCallback<Unit> OnUnitSelected { get; set; }

        private async Task SelectUnit(Unit unit)
        {
            await OnUnitSelected.InvokeAsync(unit);
            navigationManager.NavigateTo($"/Fractions/GAR/{unit.Name}");
        }
    }
}