using DbController;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Components.Calculators
{
    public partial class ForceCalculator
    {
        public int UnitId { get; set; }
        public List<Unit> Units { get; set; } = new();

        //public Task<int> StartCalculation(Unit unit)
        //{
        //    return Calculation.Force(unit);
        //}

        protected override async Task OnInitializedAsync()
        {
            using IDbController dbController = dbProviderService.GetDbController(AppdatenService.ConnectionString);

            Units = await unitService.GetAllAsync(dbController);
        }
    }
}