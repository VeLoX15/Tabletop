using DbController;

namespace Tabletop.Models
{
    public class Unit
    {
        [CompareField("unit_id")]
        public int UnitId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("fraction")]
        public string Fraction { get; set; } = string.Empty;
        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
        [CompareField("defense")]
        public int Defense { get; set; }
        [CompareField("moving")]
        public int Moving { get; set; }
        [CompareField("primary_weapon_id")]
        public Weapon PrimaryWeapon { get; set; } = new();
        [CompareField("secondary_weapon_id")]
        public Weapon SecondaryWeapon { get; set; } = new();

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "USER_ID", UnitId },
                { "NAME", Name },
                { "FRACTION", Fraction },
                { "DESCRIPTION", Description },
                { "DEFENSE", Defense },
                { "MOVING", Moving },
                { "PRIMARY_WEAPON_ID", PrimaryWeapon },
                { "SECONDARY_WEAPON_ID", SecondaryWeapon }
            };
        }
    }
}
