using DbController;
using Tabletop.Core.Interfaces;

namespace Tabletop.Core.Models
{
    public class Gamemode : LocalizationModelBase<GamemodeDescription>, IDbModel
    {
        [CompareField("gamemode_id")]
        public int GamemodeId { get; set; }

        [CompareField("image")]
        public byte[]? Image { get; set; }

        public string ConvertedImage { get; set; } = string.Empty;

        public int Id => GamemodeId;

        public IEnumerable<Dictionary<string, object?>> GetLocalizedParameters()
        {
            foreach (var description in Description)
            {
                yield return new Dictionary<string, object?>
                {
                    { "PERMISSION_ID", GamemodeId },
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
                { "GAMEMODE_ID", GamemodeId },
                { "IMAGE", Image }
            };
        }
    }

    public class GamemodeDescription : ILocalizationHelper
    {
        [CompareField("gamemode_id")]
        public int GamemodeId { get; set; }

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