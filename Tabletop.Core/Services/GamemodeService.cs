using DbController;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class GamemodeService : IModelService<Gamemode, int>
    {
        public async Task CreateAsync(Gamemode input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO `tabletop`.`gamemodes` 
                (
                `name`,
                `description`,
                `mechanic`,
                `image`
                )
                VALUES
                (
                @NAME,
                @DESCRIPTION,
                @MECHANIC,
                @IMAGE
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
    }
}
