using DbController;
using System.Globalization;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class GamemodeService : IModelService<Gamemode, int, GamemodeFilter>
    {
        public async Task CreateAsync(Gamemode input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO Gamemodes 
                (
                )
                VALUES
                (
                ); {dbController.GetLastIdSql()}";

            input.GamemodeId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"INSERT INTO GamemodeDescription
                    (
                    GamemodeId,
                    Code,
                    Name,
                    Description,
                    Mechanic
                    )
                    VALUES
                    (
                    @GAMEMODE_ID,
                    @CODE,
                    @NAME,
                    @DESCRIPTION,
                    @MECHANIC
                    )";

                var parameters = new
                {
                    GAMEMODE_ID = input.GamemodeId,
                    CODE = description.Code,
                    NAME = description.Name,
                    DESCRIPTION = description.Description
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        public async Task DeleteAsync(Gamemode input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM Gamemodes  WHERE GamemodeId = @GAMEMODE_ID";

            await dbController.QueryAsync(sql, new
            {
                GAMEMODE_ID = input.GamemodeId
            }, cancellationToken);
        }

        public async Task<Gamemode?> GetAsync(int gamemodeId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"SELECT * FROM Gamemodes  WHERE GamemodeId = @GAMEMODE_ID";

            var gamemode = await dbController.GetFirstAsync<Gamemode>(sql, new
            {
                GAMEMODE_ID = gamemodeId
            }, cancellationToken);

            return gamemode;
        }

        public static async Task<List<Gamemode>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM Gamemodes";

            var list = await dbController.SelectDataAsync<Gamemode>(sql, cancellationToken: cancellationToken);
            await LoadGamemodeDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task UpdateAsync(Gamemode input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql;
            //string sql = @"UPDATE Gamemodes SET
            //    Image = @IMAGE
            //    WHERE GamemodeId = @GAMEMODE_ID";

            //await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"UPDATE GamemodeDescription SET
                    Name = @NAME,
                    Description = @DESCRIPTION,
                    Mechanic = @MECHANIC
                    WHERE GamemodeId = @GAMEMODE_ID AND Code = @CODE";

                var parameters = new
                {
                    GAMEMODE_ID = input.GamemodeId,
                    CODE = description.Code,
                    NAME = description.Name,
                    DESCRIPTION = description.Description,
                    MECHANIC = description.Mechanic
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        public async Task<List<Gamemode>> GetAsync(GamemodeFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT gd.Name, g.* " +
                "FROM GamemodeDescription gd " +
                "INNER JOIN Gamemodes g " +
                "ON (g.GamemodeId = gd.GamemodeId) " +
                "WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");
            sqlBuilder.AppendLine(@$" ORDER BY Name ASC ");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            // Zum Debuggen schreiben wir den Wert einmal als Variabel
            string sql = sqlBuilder.ToString();

            List<Gamemode> list = await dbController.SelectDataAsync<Gamemode>(sql, GetFilterParameter(filter), cancellationToken);
            await LoadGamemodeDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task<int> GetTotalAsync(GamemodeFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) AS record_count FROM GamemodeDescription WHERE 1 = 1 ");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public string GetFilterWhere(GamemodeFilter filter)
        {
            StringBuilder sqlBuilder = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sqlBuilder.AppendLine(@" AND (UPPER(Name) LIKE @SEARCHPHRASE)");
            }

            string sql = sqlBuilder.ToString();
            return sql;
        }

        public Dictionary<string, object?> GetFilterParameter(GamemodeFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" },
                { "CULTURE", CultureInfo.CurrentCulture.Name }
            };
        }

        private static async Task LoadGamemodeDescriptionsAsync(List<Gamemode> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Count != 0)
            {
                IEnumerable<int> gamemodeIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM GamemodeDescription WHERE GamemodeId IN ({string.Join(",", gamemodeIds)})";
                List<GamemodeDescription> descriptions = await dbController.SelectDataAsync<GamemodeDescription>(sql, null, cancellationToken);

                foreach (var gamemode in list)
                {
                    gamemode.Description = descriptions.Where(x => x.GamemodeId == gamemode.Id).ToList();
                }
            }
        }
    }
}