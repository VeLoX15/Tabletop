namespace Tabletop.Pages
{
    public partial class Calculators
    {
        public int SelectedOption { get; set; } = 0;

        enum Calculator
        {
            None,
            ForceCalculator,
            ProbabilityCalculator,
            AttackValueTranslator,
            CombatSimulation
        }
    }
}