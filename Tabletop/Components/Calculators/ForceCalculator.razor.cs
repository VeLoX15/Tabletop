using Tabletop.Models;

namespace Tabletop.Components.Calculators
{
    public partial class ForceCalculator
    {
        public int UnitId { get; set; }
        public Unit unit { get; set; } = new Unit();
        public List<Unit> Units { get; set; } = new();

        //public Task<int> StartCalculation(Unit unit)
        //{
        //    return Calculation.Force(unit);
        //}
    }
}