using DbController;
using Tabletop.Core.Interfaces;

namespace Tabletop.Core.Models
{
    public class Fraction : LocalizationModelBase<FractionDescription>, IDbModel
    {
        [CompareField("fraction_id")]
        public int FractionId { get; set; }

        [CompareField("image")]
        public byte[]? Image { get; set; }

        public string ConvertedImage { get; set; } = string.Empty;

        public int Id => FractionId;

        public IEnumerable<Dictionary<string, object?>> GetLocalizedParameters()
        {
            foreach (var description in Description)
            {
                yield return new Dictionary<string, object?>
                {
                    { "PERMISSION_ID", FractionId },
                    { "NAME", description.Name },
                    { "SHORT_NAME", description.ShortName },
                    { "DESCRIPTION", description.Description }
                };
            }
        }

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "FRACTION_ID", FractionId },
                { "IMAGE", Image }
            };
        }

    }

    public class FractionDescription : ILocalizationHelper
    {
        [CompareField("fraction_id")]
        public int FractionId { get; set; }

        [CompareField("code")]
        public string Code { get; set; } = string.Empty;

        [CompareField("name")]
        public string Name { get; set; } = string.Empty;

        [CompareField("short_name")]
        public string ShortName { get; set; } = string.Empty;

        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
    }
}