using DbController;

namespace Tabletop.Core.Models
{
    public class Fraction
    {
        [CompareField("fraction_id")]
        public int FractionId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("description")]
        public string Description { get; set; } = string.Empty;

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "FRACTION_ID", FractionId },
                { "NAME", Name },
                { "DESCRIPTION", Description }
            };
        }
    }
}
