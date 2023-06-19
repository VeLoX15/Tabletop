using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class GamemodeService : IModelService<Gamemode, int, GamemodeFilter>
    {
        public async Task CreateAsync(Gamemode input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO `tabletop`.`gamemodes` 
                (
                `name`,
                `description`,
                `mechanic`
                )
                VALUES
                (
                @NAME,
                @DESCRIPTION,
                @MECHANIC
                ); {dbController.GetLastIdSql()}";

            input.GamemodeId = await dbController.GetFirstAsync<int>(sql, input.GetParameters());
        }

        public async Task DeleteAsync(Gamemode input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM `tabletop`.`gamemodes`  WHERE `gamemode_id` = @GAMEMODE_ID";

            await dbController.QueryAsync(sql, new
            {
                GAMEMODE_ID = input.GamemodeId,
            });
        }

        public async Task<Gamemode?> GetAsync(int gamemodeId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"SELECT * FROM `tabletop`.`gamemodes`  WHERE `gamemode_id` = @GAMEMODE_ID";

            var gamemode = await dbController.GetFirstAsync<Gamemode>(sql, new
            {
                GAMEMODE_ID = gamemodeId
            });

            return gamemode;
        }

        public static async Task<List<Gamemode>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM `tabletop`.`gamemodes`";

            var list = await dbController.SelectDataAsync<Gamemode>(sql);

            return list;
        }

        public async Task UpdateAsync(Gamemode input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE `tabletop`.`gamemodes` SET
                `name` = @NAME,
                `description` = @DESCRIPTION,
                `mechanic` = @MECHANIC,
                `image` = @IMAGE
                WHERE `gamemode_id` = @GAMEMODE_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);
        }

        public async Task<List<Gamemode>> GetAsync(GamemodeFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.Append("SELECT * FROM `tabletop`.`gamemodes` WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@$"  ORDER BY `name` ASC");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            // Zum Debuggen schreiben wir den Wert einmal als Variabel
            string sql = sqlBuilder.ToString();

            List<Gamemode> list = await dbController.SelectDataAsync<Gamemode>(sql, GetFilterParameter(filter), cancellationToken);

            return list;
        }

        public async Task<int> GetTotalAsync(GamemodeFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) FROM `tabletop`.`gamemodes` WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public string GetFilterWhere(GamemodeFilter filter)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sb.AppendLine(@" AND 
(
        UPPER(name) LIKE @SEARCHPHRASE
)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public Dictionary<string, object?> GetFilterParameter(GamemodeFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" }
            };
        }
    }
}
