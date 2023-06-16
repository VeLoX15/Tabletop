using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Public
{
    public partial class Fractions
    {
        private List<Fraction> List { get; set; } = new();

        protected override void OnInitialized()
        {
            List = AppdataService.Fractions.ToList();

            foreach(var fraction in List)
            {
                if (fraction.Image != null)
                {
                    string base64String = Convert.ToBase64String(fraction.Image);
                    fraction.ConvertedImage = $"data:image/png;base64,{base64String}";
                }
            }
        }
    }
}