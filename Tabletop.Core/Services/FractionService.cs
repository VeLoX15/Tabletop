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
            string sql = $@"INSERT INTO Fractions
                (
                )
                VALUES
                (
                ); {dbController.GetLastIdSql()}";

            input.FractionId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"INSERT INTO FractionDescription
                    (
                    FractionId,
                    Code,
                    Name,
                    ShortName,
                    Description
                    )
                    VALUES
                    (
                    @FRACTION_ID,
                    @CODE,
                    @NAME,
                    @SHORT_NAME,
                    @DESCRIPTION
                    )";

                var parameters = new
                {
                    FRACTION_ID = input.FractionId,
                    CODE = description.Code,
                    NAME = description.Name,
                    SHORT_NAME = description.ShortName,
                    DESCRIPTION = description.Description
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        public async Task DeleteAsync(Fraction input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM Fractions WHERE FractionId = @FRACTION_ID";

            await dbController.QueryAsync(sql, new
            {
                FRACTION_ID = input.FractionId
            }, cancellationToken);
        }

        public async Task<Fraction?> GetAsync(int fractionId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"SELECT * FROM Fractions WHERE FractionId = @FRACTION_ID";

            var fraction = await dbController.GetFirstAsync<Fraction>(sql, new
            {
                FRACTION_ID = fractionId
            }, cancellationToken);

            return fraction;
        }

        public static async Task<List<Fraction>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM Fractions";

            var list = await dbController.SelectDataAsync<Fraction>(sql, cancellationToken: cancellationToken);
            await LoadFractionDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task UpdateAsync(Fraction input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE Fractions SET
                Image = @IMAGE
                WHERE FractionId = @FRACTION_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"UPDATE FractionDescription SET
                    Name = @NAME,
                    Description = @DESCRIPTION,
                    ShortName = @SHORT_NAME
                    WHERE FractionId = @FRACTION_ID AND Code = @CODE";

                var parameters = new
                {
                    FRACTION_ID = input.FractionId,
                    CODE = description.Code,
                    NAME = description.Name,
                    SHORT_NAME = description.ShortName,
                    DESCRIPTION = description.Description
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        public async Task<List<Fraction>> GetAsync(FractionFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT fd.Name, f.* " +
                "FROM FractionDescription fd " +
                "INNER JOIN Fractions f " +
                "ON (f.FractionId = fd.FractionId) " +
                "WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");
            sqlBuilder.AppendLine(@$" ORDER BY Name ASC ");
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
            sqlBuilder.AppendLine("SELECT COUNT(*) AS record_count FROM FractionDescription WHERE 1 = 1 ");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public string GetFilterWhere(FractionFilter filter)
        {
            StringBuilder sqlBuilder = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sqlBuilder.AppendLine(@" AND (UPPER(Name) LIKE @SEARCHPHRASE)");
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
            if (list.Count != 0)
            {
                IEnumerable<int> fractionIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM FractionDescription WHERE FractionId IN ({string.Join(",", fractionIds)})";
                List<FractionDescription> descriptions = await dbController.SelectDataAsync<FractionDescription>(sql, null, cancellationToken);

                foreach (var fraction in list)
                {
                    fraction.Description = descriptions.Where(x => x.FractionId == fraction.Id).ToList();
                }
            }
        }
    }
}
