﻿using Tabletop.Core.Filters.Abstract;

namespace Tabletop.Core.Filters
{
    public class UnitFilter : PageFilterBase
    {
        public bool ShowOnlyActiveForms { get; set; }
        public bool HideLoginRequired { get; set; }
    }
}