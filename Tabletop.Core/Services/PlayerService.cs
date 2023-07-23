using DbController;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    internal class PlayerService : IModelService<Player, int>
    {
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

            input.PlayerId = await dbController.GetFirstAsync<int>(sql, input.GetParameters());
        }

        public Task DeleteAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Player?> GetAsync(int identifier, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Player input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
