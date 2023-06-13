﻿using DbController;

namespace Tabletop.Core.Models
{
    public class Unit
    {
        [CompareField("unit_id")]
        public int UnitId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("fraction_id")]
        public int FractionId { get; set; }
        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
        [CompareField("mechanic")]
        public string Mechanic { get; set; } = string.Empty;
        [CompareField("defense")]
        public int Defense { get; set; }
        [CompareField("moving")]
        public int Moving { get; set; }
        [CompareField("primary_weapon_id")]
        public int PrimaryWeaponId { get; set; }
        [CompareField("secondary_weapon_id")]
        public int SecondaryWeaponId { get; set; }

        public Weapon PrimaryWeapon { get; set; } = new();
        public Weapon SecondaryWeapon { get; set; } = new();

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "USER_ID", UnitId },
                { "NAME", Name },
                { "FRACTION_ID", FractionId },
                { "DESCRIPTION", Description },
                { "MECHANIC", Mechanic },
                { "DEFENSE", Defense },
                { "MOVING", Moving },
                { "PRIMARY_WEAPON_ID", PrimaryWeaponId },
                { "SECONDARY_WEAPON_ID", SecondaryWeaponId }
            };
        }
    }
}
