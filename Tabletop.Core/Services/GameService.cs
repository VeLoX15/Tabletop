using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class GameService : IModelService<Game, int, GameFilter>
    {
        private readonly PlayerService _playerService;

        public GameService(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task CreateAsync(Game input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = $@"INSERT INTO `tabletop`.`games` 
                (
                `user_id`,
                `gamemode_id`,
                `name`,
                `rounds`,
                `force`,
                `date`
                )
                VALUES
                (
                @USER_ID,
                @GAMEMODE_ID,
                @NAME,
                @ROUNDS,
                @FORCE,
                @DATE
                ); {dbController.GetLastIdSql()}";

            input.GameId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);
        }

        public async Task DeleteAsync(Game input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`games` WHERE `game_id` = @GAME_ID";

            await dbController.QueryAsync(sql, new
            {
                GAME_ID = input.GameId,
            }, cancellationToken);
        }

        public async Task<Game?> GetAsync(int gameId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM `tabletop`.`games`  WHERE `game_id` = @GAME_ID";

            var game = await dbController.GetFirstAsync<Game>(sql, new
            {
                GAME_ID = gameId
            }, cancellationToken);

            if (game != null)
            {
                game.Players = await _playerService.GetGamePlayersAsync(game.GameId, dbController, cancellationToken);
            }

            return game;
        }

        public static async Task<List<Game>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "SELECT * FROM `tabletop`.`games`";

            var list = await dbController.SelectDataAsync<Game>(sql, cancellationToken: cancellationToken);

            return list;
        }

        public async Task<List<Game>> GetAsync(GameFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT DISTINCT g.* FROM `games` g LEFT JOIN `players` p ON g.`game_id` = p.`game_id` WHERE p.`user_id` = @USER_ID OR g.`user_id` = @USER_ID ");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$"  ORDER BY `date` DESC ");
            sb.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sb.ToString();

            List<Game> list = await dbController.SelectDataAsync<Game>(sql, GetFilterParameter(filter), cancellationToken);

            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.Players = await _playerService.GetGamePlayersAsync(item.GameId, dbController, cancellationToken);
                }
            }

            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(GameFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" },
                { "USER_ID", filter.UserId }
            };
        }

        public string GetFilterWhere(GameFilter filter)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sb.AppendLine(@" AND (UPPER(`name`) LIKE @SEARCHPHRASE)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(GameFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM `tabletop`.`games` WHERE `user_id` = @USER_ID");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public async Task UpdateAsync(Game input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE `tabletop`.`games` SET
                `gamemode_id` = @GAMEMODE_ID,
                `name` = @NAME,
                `rounds` = @ROUNDS,
                `force` = @FORCE,
                `date` = @DATE
                WHERE `game_id` = @GAME_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var item in input.Players)
            {
                await _playerService.CreateAsync(item, dbController, cancellationToken);
            }
        }
    }
}