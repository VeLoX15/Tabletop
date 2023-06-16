using DbController;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class FractionService : IModelService<Fraction, int>
    {
        public async Task CreateAsync(Fraction input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO `tabletop`.`fractions` 
                (
                `name`,
                `short_name`,
                `description`,
                `mechanic`,
                `image`
                )
                VALUES
                (
                @NAME,
                @SHORT_NAME
                @DESCRIPTION,
                @MECHANIC,
                @IMAGE
                ); {dbController.GetLastIdSql()}";

            input.FractionId = await dbController.GetFirstAsync<int>(sql, input.GetParameters());
        }

        public async Task DeleteAsync(Fraction input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM `tabletop`.`fractions`  WHERE `fraction_id` = @FRACTION_ID";

            await dbController.QueryAsync(sql, new
            {
                FRACTION_ID = input.FractionId,
            });
        }

        public async Task<Fraction?> GetAsync(int fractionId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"SELECT * FROM `tabletop`.`fractions`  WHERE `fraction_id` = @FRACTION_ID";

            var fraction = await dbController.GetFirstAsync<Fraction>(sql, new
            {
                FRACTION_ID = fractionId
            });

            return fraction;
        }

        public static async Task<List<Fraction>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM `tabletop`.`fractions`";

            var list = await dbController.SelectDataAsync<Fraction>(sql);

            return list;
        }

        public async Task UpdateAsync(Fraction input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE `tabletop`.`fractions` SET
                `name` = @NAME,
                `short_name` = @SHORT_NAME,    
                `description` = @DESCRIPTION,
                `mechanic` = @MECHANIC,
                `image` = @IMAGE
                WHERE `fraction_id` = @FRACTION_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);
        }
    }
}
