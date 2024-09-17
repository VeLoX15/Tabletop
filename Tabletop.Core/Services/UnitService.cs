using DbController;
using System.Globalization;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class UnitService(WeaponService weaponService) : IModelService<Unit, int, UnitFilter>
    {
        private readonly WeaponService _weaponService = weaponService;

        public async Task CreateAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = $@"INSERT INTO Units 
                (
                FractionId,
                ClassId,
                TroopQuantity,
                Defense,
                Moving,
                PrimaryWeaponId,
                SecondaryWeaponId,
                FistAbilityId,
                SecondAbilityId
                )
                VALUES
                (
                @FRACTION_ID,
                @CLASS_ID,
                @TROOP_QUANTITY,
                @DEFENSE,
                @MOVING,
                @PRIMARY_WEAPON_ID,
                @SECONDARY_WEAPON_ID,
                @FIRST_ABILITY_ID,
                @SECOND_ABILITY_ID
                ); {dbController.GetLastIdSql()}";

            input.UnitId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"INSERT INTO UnitDescription
                    (
                    UnitId,
                    Code,
                    Name,
                    Description
                    )
                    VALUES
                    (
                    @UNIT_ID,
                    @CODE,
                    @NAME,
                    @DESCRIPTION
                    )";

                var parameters = new
                {
                    UNIT_ID = input.UnitId,
                    CODE = description.Code,
                    NAME = description.Name,
                    DESCRIPTION = description.Description
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        public async Task DeleteAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM Units WHERE UnitId = @UNIT_ID";

            await dbController.QueryAsync(sql, new
            {
                UNIT_ID = input.UnitId
            }, cancellationToken);
        }

        public async Task DeleteUnitAsync(int userId, int unitId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM UserUnits WHERE UserId = @USER_ID AND UnitId = @UNIT_ID";

            await dbController.QueryAsync(sql, new
            {
                USER_ID = userId,
                UNIT_ID = unitId
            }, cancellationToken);
        }

        public async Task<Unit?> GetAsync(int unitId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM Units  WHERE UnitId = @UNIT_ID";

            var unit = await dbController.GetFirstAsync<Unit>(sql, new
            {
                UNIT_ID = unitId
            }, cancellationToken);

            if (unit?.PrimaryWeaponId != null && unit?.SecondaryWeaponId != null)
            {
                unit.PrimaryWeapon = await _weaponService.GetAsync((int)unit.PrimaryWeaponId, dbController, cancellationToken) ?? new();
                unit.SecondaryWeapon = await _weaponService.GetAsync((int)unit.SecondaryWeaponId, dbController, cancellationToken) ?? new();
            }

            return unit;
        }

        public static async Task<List<Unit>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "SELECT * FROM Units";

            var list = await dbController.SelectDataAsync<Unit>(sql, cancellationToken: cancellationToken);

            await LoadUnitDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task<List<Unit>> GetAsync(UnitFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT ud.Name, u.* " +
                "FROM UnitDescription ud " +
                "INNER JOIN Units u " +
                "ON (u.UnitId = ud.UnitId) " +
                "WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");
            sqlBuilder.AppendLine(@$" ORDER BY Name ASC ");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sqlBuilder.ToString();

            List<Unit> list = await dbController.SelectDataAsync<Unit>(sql, GetFilterParameter(filter), cancellationToken);

            await LoadUnitDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(UnitFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" },
                { "FRACTION_ID", filter.FractionId },
                { "CULTURE", CultureInfo.CurrentCulture.Name }
            };
        }

        public string GetFilterWhere(UnitFilter filter)
        {
            StringBuilder sqlBuilder = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sqlBuilder.AppendLine(@" AND (UPPER(Name) LIKE @SEARCHPHRASE)");
            }
            if (filter.FractionId != 0)
            {
                sqlBuilder.AppendLine(@" AND FractionId = @FRACTION_ID");
            }

            string sql = sqlBuilder.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(UnitFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) AS record_count FROM UnitDescription ud JOIN Units u ON ud.UnitId = u.UnitId WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public static async Task<List<Unit>> GetUserUnitsAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"
                SELECT uu.Quantity, u.*
                FROM UserUnits uu
                INNER JOIN Units u ON (u.UnitId = uu.UnitId)
                WHERE uu.UserId = @USER_ID";

            var list = await dbController.SelectDataAsync<Unit>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            await LoadUnitDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public static async Task<List<Unit>> GetPlayerUnitsAsync(int playerId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"
                SELECT pu.Quantity, u.*
                FROM PlayerUnits pu
                INNER JOIN Units u ON (u.UnitId = pu.UnitId)
                WHERE pu.PlayerId = @PLAYER_ID";

            var list = await dbController.SelectDataAsync<Unit>(sql, new
            {
                PLAYER_ID = playerId
            }, cancellationToken);

            await LoadUnitDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public static async Task<List<Unit>> GetTemplateUnitsAsync(int templateId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"
                SELECT tu.Quantity, u.*
                FROM TemplateUnits tu
                INNER JOIN Units u ON (u.UnitId = tu.UnitId)
                WHERE tu.TemplateId = @TEMPLATE_ID";

            var list = await dbController.SelectDataAsync<Unit>(sql, new
            {
                TEMPLATE_ID = templateId
            }, cancellationToken);

            await LoadUnitDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public static async Task CreateUserUnitAsync(User user, Unit unit, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM UserUnits WHERE UserId = @USER_ID AND UnitId = @UNIT_ID";
            await dbController.QueryAsync(sql, new
            {
                USER_ID = user.UserId,
                UNIT_ID = unit.UnitId
            }, cancellationToken);

            sql = @"INSERT INTO UserUnits
                (
                UserId,
                UnitId,
                Quantity
                )
                VALUES
                (
                @USER_ID,
                @UNIT_ID,
                @QUANTITY
                )";

            await dbController.QueryAsync(sql, new
            {
                USER_ID = user.UserId,
                UNIT_ID = unit.UnitId,
                QUANTITY = unit.Quantity
            }, cancellationToken);
        }

        public static async Task DeleteTemplateUnitsAsync(int templateId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM TemplateUnits WHERE TemplateId = @TEMPLATE_ID";
            await dbController.QueryAsync(sql, new
            {
                TEMPLATE_ID = templateId
            }, cancellationToken);
        }

        public static async Task DeletePlayerUnitsAsync(int player_id, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM PlayerUnits WHERE PlayerId = @PLAYER_ID";
            await dbController.QueryAsync(sql, new
            {
                PLAYER_ID = player_id
            }, cancellationToken);
        }

        public static async Task CreateTemplateUnitAsync(Template template, Unit unit, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"INSERT INTO TemplateUnits
                (
                TemplateId,
                UnitId,
                Quantity
                )
                VALUES
                (
                @TEMPLATE_ID,
                @UNIT_ID,
                @QUANTITY
                )";

            await dbController.QueryAsync(sql, new
            {
                TEMPLATE_ID = template.TemplateId,
                UNIT_ID = unit.UnitId,
                QUANTITY = unit.Quantity
            }, cancellationToken);
        }

        public static async Task CreatePlayerUnitAsync(Player player, Unit unit, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"INSERT INTO PlayerUnits
                (
                PlayerId,
                UnitId,
                Quantity
                )
                VALUES
                (
                @PLAYER_ID,
                @UNIT_ID,
                @QUANTITY
                )";

            await dbController.QueryAsync(sql, new
            {
                PLAYER_ID = player.PlayerId,
                UNIT_ID = unit.UnitId,
                QUANTITY = unit.Quantity
            }, cancellationToken);
        }

        public async Task UpdateAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE Units SET
                FractionId = @FRACTION_ID,
                ClassId = @CLASS_ID,
                TroopQuantity = @TROOP_QUANTITY,
                Defense = @DEFENSE,
                Moving = @MOVING,
                PrimaryWeaponId = @PRIMARY_WEAPON_ID,
                SecondaryWeaponId = @SECONDARY_WEAPON_ID,
                FirstAbilityId = @FIRST_ABILITY_ID,
                SecondAbilityId = @SECOND_ABILITY_ID
                WHERE UnitId = @UNIT_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"UPDATE UnitDescription SET
                    Name = @NAME,
                    Description = @DESCRIPTION,
                    Mechanic = @MECHANIC
                    WHERE UnitId = @UNIT_ID AND Code = @CODE";

                var parameters = new
                {
                    UNIT_ID = input.UnitId,
                    CODE = description.Code,
                    NAME = description.Name,
                    DESCRIPTION = description.Description,
                    MECHANIC = description.Mechanic
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        private static async Task LoadUnitDescriptionsAsync(List<Unit> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Count != 0)
            {
                IEnumerable<int> unitIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM UnitDescription WHERE UnitId IN ({string.Join(",", unitIds)})";
                List<UnitDescription> descriptions = await dbController.SelectDataAsync<UnitDescription>(sql, null, cancellationToken);

                foreach (var unit in list)
                {
                    unit.Description = descriptions.Where(x => x.UnitId == unit.Id).ToList();
                }
            }
        }
    }
}