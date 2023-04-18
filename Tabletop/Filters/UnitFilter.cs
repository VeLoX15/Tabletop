using Tabletop.Filters.Abstract;

namespace Tabletop.Filters
{
    public class UnitFilter : PageFilterBase
    {
        public bool ShowOnlyActiveForms { get; set; }
        public bool HideLoginRequired { get; set; }
    }
}
