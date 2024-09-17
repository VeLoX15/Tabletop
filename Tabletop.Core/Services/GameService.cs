using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class GameService(PlayerService playerService, UserService userService) : IModelService<Game, int, GameFilter>
    {
        private readonly PlayerService _playerService = playerService;
        private readonly UserService _userService = userService;

        public async Task CreateAsync(Game input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = $@"INSERT INTO Games 
                (
                UserId,
                GamemodeId,
                Name,
                NumberOfRounds,
                Force,
                NumberOfTeams,
                NumberOfPlayers,
                Date
                )
                VALUES
                (
                @USER_ID,
                @GAMEMODE_ID,
                @NAME,
                @NUMBER_OF_ROUNDS,
                @FORCE,
                @NUMBER_OF_TEAMS,
                @NUMBER_OF_PLAYERS,
                @DATE
                ); {dbController.GetLastIdSql()}";

            input.GameId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);
        }

        public async Task DeleteAsync(Game input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM Games WHERE GameId = @GAME_ID";

            await dbController.QueryAsync(sql, new
            {
                GAME_ID = input.GameId,
            }, cancellationToken);
        }

        public async Task<Game?> GetAsync(int gameId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM Games  WHERE GameId = @GAME_ID";

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
            string sql = "SELECT * FROM Games";

            var list = await dbController.SelectDataAsync<Game>(sql, cancellationToken: cancellationToken);

            return list;
        }

        public async Task<List<Game>> GetAsync(GameFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT DISTINCT g.* FROM Games g LEFT JOIN Players p ON g.GameId = p.GameId WHERE p.UserId = @USER_ID OR g.UserId = @USER_ID ");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$" ORDER BY Date DESC ");
            sb.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sb.ToString();

            List<Game> list = await dbController.SelectDataAsync<Game>(sql, GetFilterParameter(filter), cancellationToken);

            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    item.Players = await _playerService.GetGamePlayersAsync(item.GameId, dbController, cancellationToken);
                    item.Host = await _userService.GetUsernameAsync(item.UserId, dbController, cancellationToken);
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
                sb.AppendLine(@" AND (UPPER(Name) LIKE @SEARCHPHRASE)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(GameFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM Games WHERE UserId = @USER_ID");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public async Task UpdateAsync(Game input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE Games SET
                GamemodeId = @GAMEMODE_ID,
                Name = @NAME,
                NumberOfRounds = @NUMBER_OF_ROUNDS,
                Force = @FORCE,
                NumberOfTeams = @NUMBER_OF_TEAMS,
                NumberOfPlayers = @NUMBER_OF_PLAYERS,
                Date = @DATE
                WHERE GameId = @GAME_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var item in input.Players)
            {
                await _playerService.CreateAsync(item, dbController, cancellationToken);
            }
        }
    }
}