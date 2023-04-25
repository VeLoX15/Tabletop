using DbController;
using Tabletop.Models;
using Tabletop.Services;

namespace Tabletop.Components.Calculators
{
    public partial class AttackValueTranslator
    {
        public int AttackerUnitId { get; set; }
        public int DefenderUnitId { get; set; }
        public List<Unit> Units { get; set; } = new();

        //public Task<int> StartCalculation(Unit unit)
        //{
        //    return Calculation.AttackValueTranslator(unit);
        //}

        protected override async Task OnInitializedAsync()
        {
            using IDbController dbController = dbProviderService.GetDbController(AppdatenService.ConnectionString);

            Units = await unitService.GetAllAsync(dbController);
        }
    }
}