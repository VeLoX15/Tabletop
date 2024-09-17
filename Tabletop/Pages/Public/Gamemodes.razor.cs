using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Public
{
    public partial class Gamemodes
    {
        private List<Gamemode> List { get; set; } = new();

        protected override void OnInitialized()
        {
            List = [.. AppdataService.Gamemodes];

            foreach (var gamemode in List)
            {
                if (gamemode.Image != null)
                {
                    string base64String = Convert.ToBase64String(gamemode.Image);
                    gamemode.ConvertedImage = $"data:image/png;base64,{base64String}";
                }
            }
        }
    }
}
