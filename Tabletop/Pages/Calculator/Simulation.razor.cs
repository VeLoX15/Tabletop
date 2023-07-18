using Tabletop.Core.Constants;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Calculator
{
    public partial class Simulation
    {
        public int FractionIdUnit1 { get; set; } = 1;
        public int UnitIdUnit1 { get; set; } = 1;
        public int FractionIdUnit2 { get; set; } = 1;
        public int UnitIdUnit2 { get; set; } = 1;
        public string CoverUnit1 { get; set; } = string.Empty;
        public string CoverUnit2 { get; set; } = string.Empty;
        public int QuantityUnit1 { get; set; }
        public int QuantityUnit2 { get; set; }
        public int Distance { get; set; }
        public List<Fraction> Fractions { get; set; } = new();
        public List<Unit> Units { get; set; } = new();
        public CoverTypes CoverTypes { get; set; }

        protected override void OnInitialized()
        {
            Fractions = AppdataService.Fractions;
            Units = AppdataService.Units;
        }
    }
}