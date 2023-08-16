using DbController;
using Tabletop.Core.Interfaces;

namespace Tabletop.Core.Models
{
    public class Unit : LocalizationModelBase<UnitDescription>, IDbModel
    {
        [CompareField("unit_id")]
        public int UnitId { get; set; }

        [CompareField("fraction_id")]
        public int FractionId { get; set; }

        [CompareField("defense")]
        public int Defense { get; set; }

        [CompareField("moving")]
        public int Moving { get; set; }

        [CompareField("primary_weapon_id")]
        public int? PrimaryWeaponId { get; set; }

        [CompareField("secondary_weapon_id")]
        public int? SecondaryWeaponId { get; set; }

        [CompareField("image")]
        public byte[]? Image { get; set; }

        public int Force { get; set; }

        public string ConvertedImage { get; set; } = string.Empty;

        public Weapon? PrimaryWeapon { get; set; }
        public Weapon? SecondaryWeapon { get; set; }
        public Fraction Fraction { get; set; } = new();

        [CompareField("quantity")]
        public int Quantity { get; set; }

        public int Id => UnitId;

        public IEnumerable<Dictionary<string, object?>> GetLocalizedParameters()
        {
            foreach (var description in Description)
            {
                yield return new Dictionary<string, object?>
                {
                    { "UNIT_ID", UnitId },
                    { "NAME", description.Name },
                    { "DESCRIPTION", description.Description },
                    { "MECHANIC", description.Mechanic }
                };
            }
        }

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "UNIT_ID", UnitId },
                { "FRACTION_ID", FractionId },
                { "DEFENSE", Defense },
                { "MOVING", Moving },
                { "PRIMARY_WEAPON_ID", PrimaryWeaponId },
                { "SECONDARY_WEAPON_ID", SecondaryWeaponId },
                { "QUANTITY", Quantity }
            };
        }
    }

    public class UnitDescription : ILocalizationHelper
    {
        [CompareField("unit_id")]
        public int UnitId { get; set; }

        [CompareField("code")]
        public string Code { get; set; } = string.Empty;

        [CompareField("name")]
        public string Name { get; set; } = string.Empty;

        [CompareField("description")]
        public string Description { get; set; } = string.Empty;

        [CompareField("mechanic")]
        public string Mechanic { get; set; } = string.Empty;
    }
}