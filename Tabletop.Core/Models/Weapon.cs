using DbController;
using Tabletop.Core.Interfaces;

namespace Tabletop.Core.Models
{
    public class Weapon : LocalizationModelBase<WeaponDescription>, IDbModel
    {
        [CompareField("weapon_id")]
        public int WeaponId { get; set; }

        [CompareField("attack")]
        public int Attack { get; set; }

        [CompareField("quality")]
        public int Quality { get; set; }

        [CompareField("range")]
        public int Range { get; set; }

        [CompareField("dices")]
        public int Dices { get; set; }

        public int Id => WeaponId;

        public IEnumerable<Dictionary<string, object?>> GetLocalizedParameters()
        {
            foreach (var description in Description)
            {
                yield return new Dictionary<string, object?>
                {
                    { "PERMISSION_ID", WeaponId },
                    { "NAME", description.Name },
                    { "DESCRIPTION", description.Description }
                };
            }
        }

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "WEAPON_ID", WeaponId },
                { "ATTACK", Attack },
                { "QUALITY", Quality },
                { "RANGE", Range },
                { "DICES", Dices }
            };
        }
    }

    public class WeaponDescription : ILocalizationHelper
    {
        [CompareField("weapon_id")]
        public int WeaponId { get; set; }

        [CompareField("code")]
        public string Code { get; set; } = string.Empty;

        [CompareField("name")]
        public string Name { get; set; } = string.Empty;

        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
    }
}
