using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;

namespace Tabletop.Pages.Public
{
    public partial class UnitProfiles
    {
        public Unit Unit { get; set; } = new();
        [Parameter]
        public string UnitName { get; set; } = string.Empty;
    }
}