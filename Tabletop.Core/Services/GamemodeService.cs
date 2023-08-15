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
            string sql = $@"INSERT INTO `tabletop`.`gamemodes` 
                (
                )
                VALUES
                (
                ); {dbController.GetLastIdSql()}";

            input.GamemodeId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"INSERT INTO `tabletop`.`gamemode_description`
                    (
                    `gamemode_id`,
                    `code`,
                    `name`,
                    `description`
                    )
                    VALUES
                    (
                    @GAMEMODE_ID,
                    @CODE,
                    @NAME,
                    @DESCRIPTION
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
            string sql = "DELETE FROM `tabletop`.`gamemodes`  WHERE `gamemode_id` = @GAMEMODE_ID";

            await dbController.QueryAsync(sql, new
            {
                GAMEMODE_ID = input.GamemodeId,
            }, cancellationToken);
        }

        public async Task<Gamemode?> GetAsync(int gamemodeId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"SELECT * FROM `tabletop`.`gamemodes`  WHERE `gamemode_id` = @GAMEMODE_ID";

            var gamemode = await dbController.GetFirstAsync<Gamemode>(sql, new
            {
                GAMEMODE_ID = gamemodeId
            }, cancellationToken);

            return gamemode;
        }

        public static async Task<List<Gamemode>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM `tabletop`.`gamemodes`";

            var list = await dbController.SelectDataAsync<Gamemode>(sql, cancellationToken: cancellationToken);
            await LoadGamemodeDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task UpdateAsync(Gamemode input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE `tabletop`.`gamemodes` SET
                `image` = @IMAGE
                WHERE `gamemode_id` = @GAMEMODE_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"UPDATE `tabletop`.`gamemode_description` SET
                    `name` = @NAME,
                    `description` = @DESCRIPTION,
                    WHERE `gamemode_id` = @GAMEMODE_ID AND `code` = @CODE";

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

        public async Task<List<Gamemode>> GetAsync(GamemodeFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT gd.`name`, g.* " +
                "FROM `tabletop`.`gamemode_description` gd " +
                "INNER JOIN `tabletop`.`gamemodes` g " +
                "ON (g.`gamemode_id` = gd.`gamemode_id`) " +
                "WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND `code` = @CULTURE");
            sqlBuilder.AppendLine(@$" ORDER BY `name` ASC ");
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
            sqlBuilder.AppendLine("SELECT COUNT(*) AS record_count FROM `tabletop`.`gamemode_description` WHERE 1 = 1 ");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND `code` = @CULTURE");

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public string GetFilterWhere(GamemodeFilter filter)
        {
            StringBuilder sqlBuilder = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sqlBuilder.AppendLine(@" AND (UPPER(`name`) LIKE @SEARCHPHRASE)");
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
            if (list.Any())
            {
                IEnumerable<int> gamemodeIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM `tabletop`.`gamemode_description` WHERE `gamemode_id` IN ({string.Join(",", gamemodeIds)})";
                List<GamemodeDescription> descriptions = await dbController.SelectDataAsync<GamemodeDescription>(sql, null, cancellationToken);

                foreach (var gamemode in list)
                {
                    gamemode.Description = descriptions.Where(x => x.GamemodeId == gamemode.Id).ToList();
                }
            }
        }
    }
}