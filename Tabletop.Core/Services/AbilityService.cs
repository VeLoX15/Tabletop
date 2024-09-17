using DbController;
using System.Globalization;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class AbilityService : IModelService<Ability, int, AbilityFilter>
    {
        public async Task CreateAsync(Ability input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO Fractions
                (
                Quality,
                Force
                )
                VALUES
                (
                @QUALITY,
                @FORCE
                ); {dbController.GetLastIdSql()}";

            input.AbilityId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"INSERT INTO AbilityDescription
                    (
                    AbilityId,
                    Code,
                    Name,
                    Description,
                    Mechanic
                    )
                    VALUES
                    (
                    @ABILITY_ID,
                    @CODE,
                    @NAME,
                    @DESCRIPTION,
                    @MECHANIC
                    )";

                var parameters = new
                {
                    ABILTIY_ID = input.AbilityId,
                    CODE = description.Code,
                    NAME = description.Name,
                    DESCRIPTION = description.Description
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        public async Task DeleteAsync(Ability input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM Abilities WHERE AbilityId = @ABILITY_ID";

            await dbController.QueryAsync(sql, new
            {
                ABILITY_ID = input.AbilityId
            }, cancellationToken);
        }

        public static async Task<List<Ability>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM Abilities";

            var list = await dbController.SelectDataAsync<Ability>(sql, cancellationToken: cancellationToken);
            await LoadClassDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task<Ability?> GetAsync(int abilityId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"SELECT * FROM Abilities WHERE AbilityId = @ABILITY_ID";

            var ability = await dbController.GetFirstAsync<Ability>(sql, new
            {
                ABILITY_ID = abilityId
            }, cancellationToken);

            return ability;
        }

        private static async Task LoadClassDescriptionsAsync(List<Ability> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Count != 0)
            {
                IEnumerable<int> abilityIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM AbilityDescription WHERE AbilityId IN ({string.Join(",", abilityIds)})";
                List<AbilityDescription> descriptions = await dbController.SelectDataAsync<AbilityDescription>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.Description = descriptions.Where(x => x.AbilityId == item.Id).ToList();
                }
            }
        }

        public async Task UpdateAsync(Ability input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE Abilities SET
                Quality = @QUALITY,
                Force = @FORCE
                WHERE AbilityId = @ABILITY_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"UPDATE AbilityDescription SET
                    Name = @NAME,
                    Description = @DESCRIPTION,
                    Mechanic = @MECHANIC
                    WHERE AbilityId = @ABILITY_ID AND Code = @CODE";

                var parameters = new
                {
                    ABILITY_ID = input.AbilityId,
                    CODE = description.Code,
                    NAME = description.Name,
                    DESCRIPTION = description.Description,
                    MECHANIC = description.Mechanic
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        public async Task<List<Ability>> GetAsync(AbilityFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT ad.Name, a.* " +
                "FROM AbilityDescription ad " +
                "INNER JOIN Abilities a " +
                "ON (a.AbilityId = ad.AbilityId) " +
                "WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");
            sqlBuilder.AppendLine(@$" ORDER BY Name ASC ");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            // Zum Debuggen schreiben wir den Wert einmal als Variabel
            string sql = sqlBuilder.ToString();

            List<Ability> list = await dbController.SelectDataAsync<Ability>(sql, GetFilterParameter(filter), cancellationToken);
            await LoadAbilityDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task<int> GetTotalAsync(AbilityFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) AS record_count FROM AbilityDescription WHERE 1 = 1 ");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public string GetFilterWhere(AbilityFilter filter)
        {
            StringBuilder sqlBuilder = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sqlBuilder.AppendLine(@" AND (UPPER(Name) LIKE @SEARCHPHRASE)");
            }

            string sql = sqlBuilder.ToString();
            return sql;
        }

        public Dictionary<string, object?> GetFilterParameter(AbilityFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" },
                { "CULTURE", CultureInfo.CurrentCulture.Name }
            };
        }

        private static async Task LoadAbilityDescriptionsAsync(List<Ability> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Count != 0)
            {
                IEnumerable<int> abilityIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM AbilityDescription WHERE AbilityId IN ({string.Join(",", abilityIds)})";
                List<AbilityDescription> descriptions = await dbController.SelectDataAsync<AbilityDescription>(sql, null, cancellationToken);

                foreach (var ability in list)
                {
                    ability.Description = descriptions.Where(x => x.AbilityId == ability.Id).ToList();
                }
            }
        }
    }
}
