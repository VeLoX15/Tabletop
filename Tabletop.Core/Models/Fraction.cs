using DbController;

namespace Tabletop.Core.Models
{
    public class Fraction : IDbModel
    {
        [CompareField("fraction_id")]
        public int FractionId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("short_name")]
        public string ShortName { get; set; } = string.Empty;
        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
        [CompareField("mechanic")]
        public string Mechanic { get; set; } = string.Empty;
        [CompareField("image")]
        public byte[]? Image { get; set; }

        public string ConvertedImage { get; set; } = string.Empty;

        public int Id => FractionId;

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "FRACTION_ID", FractionId },
                { "NAME", Name },
                { "SHORT_NAME", ShortName },
                { "DESCRIPTION", Description },
                { "MECHANIC", Mechanic },
                { "IMAGE", Image },
            };
        }
    }
}
