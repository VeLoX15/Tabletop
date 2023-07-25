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
        public Game? Game { get; set; }

        //protected override void OnInitialized()
        //{
        //    using IDbController dbController = new MySqlController(AppdataService.ConnectionString);

        //    Games = gameService.GetAllAsync(dbController);
        //}
    }
}