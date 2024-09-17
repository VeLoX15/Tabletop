using Tabletop.Core.Filters.Abstract;

namespace Tabletop.Core.Filters
{
    public class TemplateFilter : PageFilterBase
    {
        public int UserId { get; set; }
        public int FractionId { get; set; }
        public int Force { get; set; }
    }
}