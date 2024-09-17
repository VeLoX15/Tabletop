using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Tabletop.Core.Calculators;
using Tabletop.Core.Constants;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Tools
{
    public partial class Simulation
    {
        public int FractionId1 { get; set; } = new();
        public int UnitId1 { get; set; } = new();
        public int FractionId2 { get; set; } = new();
        public int UnitId2 { get; set; } = new();
        public CoverTypes Cover1 { get; set; }
        public CoverTypes Cover2 { get; set; }
        public int Quantity1 { get; set; }
        public int Quantity2 { get; set; }
        public int Distance { get; set; }
        public List<Fraction> Fractions { get; set; } = [];
        public List<Unit> Units { get; set; } = [];
        public List<string> Log { get; set; } = [];

#nullable disable
        [Inject] public IJSRuntime JSRuntime { get; set; }
#nullable enable

        protected override void OnInitialized()
        {
            Fractions = AppdataService.Fractions;
            Units = AppdataService.Units;
        }

        protected async Task StartSimulation()
        {
            Log = await Calculation.Simulation(Units.FirstOrDefault(x => x.Id == UnitId1) ?? new(), Quantity1, Cover1, Units.FirstOrDefault(x => x.Id == UnitId2) ?? new(), Quantity2, Cover2, Distance);
            if (Log.Count > 1)
            {
                await JSRuntime.ShowToastAsync(ToastType.success, "Simulation finished");
            }
            else
            {
                await JSRuntime.ShowToastAsync(ToastType.warning, "Simulation aborted");
            }
        }
    }
}