using Microsoft.AspNetCore.Components;
using System.Globalization;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Public
{
    public partial class GamemodeProfile
    {
        [Parameter]
        public string GamemodeName { get; set; } = string.Empty;
        public Gamemode Gamemode { get; set; } = new();

        protected override void OnInitialized()
        {
            Gamemode = AppdataService.Gamemodes.FirstOrDefault(x => x.GetLocalization(AppdataService.SupportedCultures[0])?.Name == GamemodeName) ?? new();

            if (Gamemode.Image != null)
            {
                string base64String = Convert.ToBase64String(Gamemode.Image);
                Gamemode.ConvertedImage = $"data:image/png;base64,{base64String}";
            }
        }
    }
}