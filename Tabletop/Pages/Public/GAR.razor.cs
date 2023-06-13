using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Public
{
    public partial class GAR
    {
        List<Unit> List { get; set; } = new();
        protected override void OnInitialized() => List = AppdatenService.Units.ToList();
    }
}