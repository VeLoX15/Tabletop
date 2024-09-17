using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Public
{
    public partial class UnitOverview
    {
        [Parameter]
        public string FractionName { get; set; } = string.Empty;
        List<Unit> List { get; set; } = [];
        Fraction Fraction { get; set; } = new();

        protected override void OnInitialized()
        {
            Fraction = AppdataService.Fractions.FirstOrDefault(x => x.GetLocalization(AppdataService.SupportedCultures[0])?.ShortName == FractionName) ?? new();

            List = AppdataService.Units.Where(x => x.FractionId == Fraction.FractionId).ToList();

            foreach (var unit in List)
            {
                if (unit.Image != null)
                {
                    string base64String = Convert.ToBase64String(unit.Image);
                    unit.ConvertedImage = $"data:image/png;base64,{base64String}";
                }
            }
        }
    }
}