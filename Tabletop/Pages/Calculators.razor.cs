using DbController;
using Tabletop.Models;
using Tabletop.Services;

namespace Tabletop.Pages
{
    public partial class Calculators
    {
        public int SelectedOption { get; set; } = 0;

        public List<Unit> Units { get; set; } = new();

        Task SomeStartupTask()
        {
            // Do some task based work
            return Task.CompletedTask;
        }
    }
}