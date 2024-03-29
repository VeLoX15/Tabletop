﻿using DbController;

namespace Tabletop.Core.Models
{
    public class Template : IDbModel
    {
        [CompareField("template_id")]
        public int TemplateId { get; set; }
        [CompareField("user_id")]
        public int UserId { get; set; }
        [CompareField("fraction_id")]
        public int FractionId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("force")]
        public int Force { get; set; }
        [CompareField("used_force")]
        public int UsedForce { get; set; }
        public List<Unit> Units { get; set; } = new();

        public int Id => TemplateId;
        public int TotalUnits { get; set; }

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "TEMPLATE_ID", TemplateId },
                { "USER_ID", UserId },
                { "FRACTION_ID", FractionId },
                { "NAME", Name},
                { "FORCE", Force },
                { "USED_FORCE", UsedForce }
            };
        }
    }
}