using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Public
{
    public partial class UnitOverview
    {
        [Parameter]
        public string FractionName { get; set; } = string.Empty;
        List<Unit> List { get; set; } = new();

        protected override void OnInitialized()
        {
            List = AppdataService.Units.Where(x => x.Fraction.ShortName == FractionName).ToList();

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