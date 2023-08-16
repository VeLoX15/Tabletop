using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Public
{
    public partial class UnitProfile
    {
        [Parameter]
        public string FractionName { get; set; } = string.Empty;
        [Parameter]
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }

        protected override void OnInitialized()
        {
            Unit = AppdataService.Units.FirstOrDefault(x => x.UnitId == UnitId);

            if(Unit != null)
            {
                Unit.PrimaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == Unit?.PrimaryWeaponId) ?? null;
                Unit.SecondaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == Unit?.SecondaryWeaponId) ?? null;
            }

            if (Unit?.Image != null)
            {
                string base64String = Convert.ToBase64String(Unit.Image);
                Unit.ConvertedImage = $"data:image/png;base64,{base64String}";
            }
        }
    }
}