using DbController;
using Tabletop.Core.Interfaces;

namespace Tabletop.Core.Models
{
    public class Class : LocalizationModelBase<ClassDescription>
    {
        [CompareField("class_id")]
        public int ClassId { get; set; }

        [CompareField("quantity")]
        public string Quantity { get; set; } = string.Empty;

        public int Id => ClassId;

        public IEnumerable<Dictionary<string, object?>> GetLocalizedParameters()
        {
            foreach (var description in Description)
            {
                yield return new Dictionary<string, object?>
                {
                    { "CLASS_ID", ClassId },
                    { "NAME", description.Name },
                    { "DESCRIPTION", description.Description }
                };
            }
        }

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "CLASS_ID", ClassId },
                { "QUANTITY", Quantity }
            };
        }
    }

    public class ClassDescription : ILocalizationHelper
    {
        [CompareField("class_id")]
        public int ClassId { get; set; }

        [CompareField("code")]
        public string Code { get; set; } = string.Empty;

        [CompareField("name")]
        public string Name { get; set; } = string.Empty;

        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
    }
}
