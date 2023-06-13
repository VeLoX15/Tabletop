using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;

namespace Tabletop.Components.Cards
{
    public partial class UnitProfile
    {
        [Parameter]
        public string UnitName { get; set; } = string.Empty;

        private Unit Unit { get; set; } = new();

        //protected override void OnInitialized()
        //{
        //    UnitName = Unit.Name;
        //}

        //private void HandleUnitSelected(Unit unit)
        //{
        //    Unit = unit;
        //}
    }
}