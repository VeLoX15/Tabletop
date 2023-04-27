using DbController;

namespace Tabletop.Core.Models
{
    public class Weapon
    {
        [CompareField("weapon_id")]
        public int WeaponId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
        [CompareField("attack")]
        public int Attack { get; set; }
        [CompareField("quality")]
        public int Quality { get; set; }
        [CompareField("range")]
        public int Range { get; set; }
        [CompareField("dices")]
        public int Dices { get; set; }

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "WEAPON_ID", WeaponId },
                { "NAME", Name },                
                { "DESCRIPTION", Description },
                { "ATTACK", Attack },
                { "QUALITY", Quality },
                { "RANGE", Range },
                { "DICES", Dices }
            };
        }
    }
}
