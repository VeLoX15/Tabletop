using Microsoft.AspNetCore.Components;
using Tabletop.Core.Calculators;
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

        protected override async Task OnInitializedAsync()
        {
            Unit = AppdataService.Units.FirstOrDefault(x => x.UnitId == UnitId);

            if(Unit != null)
            {
                Unit.PrimaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == Unit?.PrimaryWeaponId);
                Unit.SecondaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == Unit?.SecondaryWeaponId);
                Unit.Class = AppdataService.Classes.FirstOrDefault(x => x.ClassId == Unit?.ClassId);
                Unit.Ability = AppdataService.Abilities.FirstOrDefault(x => x.AbilityId == Unit?.AbilityId);

                Unit.Force = await Calculation.ForceAsync(Unit);

                if (Unit?.Image != null)
                {
                    string base64String = Convert.ToBase64String(Unit.Image);
                    Unit.ConvertedImage = $"data:image/png;base64,{base64String}";
                }
            }
        }
    }
}