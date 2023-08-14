using DbController;
using System.Globalization;
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
                )
                VALUES
                (
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
            await LoadFractionDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task UpdateAsync(Fraction input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE `tabletop`.`fractions` SET
                `image` = @IMAGE
                WHERE `fraction_id` = @FRACTION_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);
        }

        public async Task<List<Fraction>> GetAsync(FractionFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT fd.`name`, f.* " +
                "FROM `tabletop`.`fraction_description` fd " +
                "INNER JOIN `tabletop`.`fractions` f " +
                "ON (f.`fraction_id` = fd.`fraction_id`) " +
                "WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND `code` = @CULTURE");
            sqlBuilder.AppendLine(@$" ORDER BY `name` ASC ");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            // Zum Debuggen schreiben wir den Wert einmal als Variabel
            string sql = sqlBuilder.ToString();

            List<Fraction> list = await dbController.SelectDataAsync<Fraction>(sql, GetFilterParameter(filter), cancellationToken);
            await LoadFractionDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task<int> GetTotalAsync(FractionFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) AS record_count FROM `tabletop`.`fraction_description` WHERE 1 = 1 ");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND `code` = @CULTURE");

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public string GetFilterWhere(FractionFilter filter)
        {
            StringBuilder sqlBuilder = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sqlBuilder.AppendLine(@" AND (UPPER(`name`) LIKE @SEARCHPHRASE)");
            }

            string sql = sqlBuilder.ToString();
            return sql;
        }

        public Dictionary<string, object?> GetFilterParameter(FractionFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" },
                { "CULTURE", CultureInfo.CurrentCulture.Name }
            };
        }

        private static async Task LoadFractionDescriptionsAsync(List<Fraction> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Any())
            {
                IEnumerable<int> fractionIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM `tabletop`.`fraction_description` WHERE `fraction_id` IN ({string.Join(",", fractionIds)})";
                List<FractionDescription> descriptions = await dbController.SelectDataAsync<FractionDescription>(sql, null, cancellationToken);

                foreach (var fraction in list)
                {
                    fraction.Description = descriptions.Where(x => x.FractionId == fraction.Id).ToList();
                }
            }
        }
    }
}
