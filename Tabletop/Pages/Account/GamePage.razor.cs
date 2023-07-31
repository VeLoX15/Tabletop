using DbController.MySql;
using DbController;
using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Account
{
    public partial class GamePage
    {
        [Parameter]
        public int GameId { get; set; }
        public Game? Input { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);

            Input = await gameService.GetAsync(GameId, dbController);
        }
    }
}