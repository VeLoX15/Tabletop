using DbController;
using System.Text;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class PlayerService : IModelService<Player, int>
    {
        private readonly UnitService _unitService;
        private readonly UserService _userService;

        public PlayerService(UnitService unitService, UserService userService)
        {
            _unitService = unitService;
            _userService = userService;
        }

        public async Task CreateAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO `tabletop`.`players` 
                (
                `user_id`,
                `game_id`,
                `fraction_id`
                )
                VALUES
                (
                @USER_ID,
                @GAME_ID,
                @FRACTION_ID
                ); {dbController.GetLastIdSql()}";

            input.PlayerId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);
        }

        public Task DeleteAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Player?> GetAsync(int identifier, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Player>> GetGamePlayersAsync(int gameId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = ("SELECT * FROM `tabletop`.`players` WHERE `game_id` = @GAME_ID");

            List<Player> list = await dbController.SelectDataAsync<Player>(sql, new
            {
                GAME_ID = gameId
            }, cancellationToken);

            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.StartUnits = await _unitService.GetPlayerUnitsAsync(gameId, dbController, cancellationToken);
                    item.User = await _userService.GetAsync(item.UserId, dbController, cancellationToken) ?? new();
                }
            }

            return list;
        }

        public Task UpdateAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
