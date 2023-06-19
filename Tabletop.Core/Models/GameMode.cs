using DbController;

namespace Tabletop.Core.Models
{
    public class Gamemode : IDbModel
    {
        [CompareField("gamemode_id")]
        public int GamemodeId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
        [CompareField("mechanic")]
        public string Mechanic { get; set; } = string.Empty;
        [CompareField("image")]
        public byte[]? Image { get; set; }

        public string ConvertedImage { get; set; } = string.Empty;

        public int Id => GamemodeId;

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "GAMEMODE_ID", GamemodeId },
                { "NAME", Name },
                { "DESCRIPTION", Description },
                { "MECHANIC", Mechanic },
                { "IMAGE", Image }
            };
        }
    }
}
