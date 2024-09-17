using DbController;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class PlayerService : IModelService<Player, int>
    {
        private readonly UnitService _unitService;
        private readonly UserService _userService;

        }

        public async Task CreateAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO `tabletop`.`players` 
                (
                `user_id`,
                `game_id`,
                `team`,
                `used_force`
                )
                VALUES
                (
                @USER_ID,
                @GAME_ID,
                @TEAM,
                @USED_FORCE
                ); {dbController.GetLastIdSql()}";

            input.PlayerId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);
        }

        public async Task DeleteAsync(Player player, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`players` WHERE `player_id` = @PLAYER_ID";

            await dbController.QueryAsync(sql, new
            {
                PLAYER_ID = player.PlayerId
            }, cancellationToken);
        }

        public async Task DeleteByGameAsync(int gameId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`players` WHERE `game_id` = @GAME_ID";

            await dbController.QueryAsync(sql, new
            {
                GAME_ID = gameId
            }, cancellationToken);
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
                    item.StartUnits = await _unitService.GetPlayerUnitsAsync(item.PlayerId, dbController, cancellationToken);
                    item.User = await _userService.GetUserForPlayerAsync(item.UserId, dbController, cancellationToken) ?? new();
                }
            }

            return list;
        }

        public async Task UpdateAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _unitService.DeletePlayerUnitsAsync(input.PlayerId, dbController, cancellationToken);

            string sql = @"UPDATE `tabletop`.`players` SET
                `user_id` = @USER_ID,
                `game_id` = @GAME_ID,
                `fraction_id` = @FRACTION_ID,
                `team` = @TEAM,
                `used_force` = @USED_FORCE
                WHERE `player_id` = @PLAYER_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var unit in input.StartUnits)
            {
                await _unitService.CreatePlayerUnitAsync(input, unit, dbController, cancellationToken);
            }
        }
    }
}
