using DbController.MySql;
using DbController;
using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;
using Tabletop.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tabletop.Pages.Account
{
    public partial class GamePage
    {
        [Parameter]
        public int GameId { get; set; }

        public Game? Game { get; set; }

        protected override void OnInitialized()
        {
            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
            var game = gameService.GetAsync(GameId, dbController);

            Game = game;
        }
    }
}