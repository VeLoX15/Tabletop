using DbController;
using Tabletop.Core.Interfaces;

namespace Tabletop.Core.Models
{
    public class Ability : LocalizationModelBase<AbilityDescription>
    {
        [CompareField("ability_id")]
        public int AbilityId { get; set; }

        [CompareField("quality")]
        public int Quality { get; set; }
        [CompareField("force")]
        public int Force { get; set; }

        public int Id => AbilityId;

        public IEnumerable<Dictionary<string, object?>> GetLocalizedParameters()
        {
            foreach (var description in Description)
            {
                yield return new Dictionary<string, object?>
                {
                    { "ABILITY_ID", AbilityId },
                    { "NAME", description.Name },
                    { "DESCRIPTION", description.Description }
                };
            }
        }

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "ABILITY_ID", AbilityId },
                { "QUALITY", Quality },
                { "FORCE", Force }
            };
        }
    }

    public class AbilityDescription : ILocalizationHelper
    {
        [CompareField("ability_id")]
        public int AbilityId { get; set; }

        [CompareField("code")]
        public string Code { get; set; } = string.Empty;

        [CompareField("name")]
        public string Name { get; set; } = string.Empty;

        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
    }
}
