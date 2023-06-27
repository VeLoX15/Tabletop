using Tabletop.Core.Constants;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Calculator
{
    public partial class Probability
    {
        public int FractionIdAttacker { get; set; } = 1;
        public int UnitIdAttacker { get; set; } = 1;
        public int FractionIdDefender { get; set; } = 1;
        public int UnitIdDefender { get; set; } = 1;
        public string Cover { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Range { get; set; }
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