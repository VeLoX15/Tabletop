using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class FractionService : IModelService<Fraction, int, FractionFilter>
    {
        public async Task CreateAsync(Fraction input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO `tabletop`.`fractions` 
                (
                `name`,
                `short_name`,
                `description`
                )
                VALUES
                (
                @NAME,
                @SHORT_NAME,
                @DESCRIPTION
                ); {dbController.GetLastIdSql()}";

            input.FractionId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);
        }

        public async Task DeleteAsync(Fraction input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM `tabletop`.`fractions`  WHERE `fraction_id` = @FRACTION_ID";

            await dbController.QueryAsync(sql, new
            {
                FRACTION_ID = input.FractionId,
            }, cancellationToken);
        }

        public async Task<Fraction?> GetAsync(int fractionId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"SELECT * FROM `tabletop`.`fractions`  WHERE `fraction_id` = @FRACTION_ID";

            var fraction = await dbController.GetFirstAsync<Fraction>(sql, new
            {
                FRACTION_ID = fractionId
            }, cancellationToken);

            return fraction;
        }

        public static async Task<List<Fraction>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM `tabletop`.`fractions`";

            var list = await dbController.SelectDataAsync<Fraction>(sql, cancellationToken: cancellationToken);

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

        public async Task<List<Fraction>> GetAsync(FractionFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.Append("SELECT * FROM `tabletop`.`fractions` WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@$"  ORDER BY `name` ASC");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            // Zum Debuggen schreiben wir den Wert einmal als Variabel
            string sql = sqlBuilder.ToString();

            List<Fraction> list = await dbController.SelectDataAsync<Fraction>(sql, GetFilterParameter(filter), cancellationToken);

            return list;
        }

        public async Task<int> GetTotalAsync(FractionFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) FROM `tabletop`.`fractions` WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public string GetFilterWhere(FractionFilter filter)
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

        public Dictionary<string, object?> GetFilterParameter(FractionFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" }
            };
        }
    }
}
