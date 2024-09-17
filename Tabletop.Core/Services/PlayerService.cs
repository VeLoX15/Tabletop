using DbController;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class PlayerService : IModelService<Player, int>
    {
        public async Task CreateAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO Players 
                (
                UserId,
                GameId,
                Team,
                UsedForce
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
            string sql = "DELETE FROM Players WHERE PlayerId = @PLAYER_ID";

            await dbController.QueryAsync(sql, new
            {
                PLAYER_ID = player.PlayerId
            }, cancellationToken);
        }

        public static async Task DeleteByGameAsync(int gameId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM Players WHERE GameId = @GAME_ID";

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
            string sql = ("SELECT * FROM Players WHERE GameId = @GAME_ID");

            List<Player> list = await dbController.SelectDataAsync<Player>(sql, new
            {
                GAME_ID = gameId
            }, cancellationToken);

            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    item.StartUnits = await UnitService.GetPlayerUnitsAsync(item.PlayerId, dbController, cancellationToken);
                    item.User = await UserService.GetUserForPlayerAsync(item.UserId, dbController, cancellationToken) ?? new();
                }
            }

            return list;
        }

        public async Task UpdateAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await UnitService.DeletePlayerUnitsAsync(input.PlayerId, dbController, cancellationToken);

            string sql = @"UPDATE Players SET
                UserId = @USER_ID,
                GameId = @GAME_ID,
                FractionId = @FRACTION_ID,
                Team = @TEAM,
                UsedForce = @USED_FORCE
                WHERE PlayerId = @PLAYER_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var unit in input.StartUnits)
            {
                await UnitService.CreatePlayerUnitAsync(input, unit, dbController, cancellationToken);
            }
        }
    }
}
