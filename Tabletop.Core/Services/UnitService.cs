using DbController;
using Org.BouncyCastle.Asn1.X509;
using System.Globalization;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class UnitService : IModelService<Unit, int, UnitFilter>
    {
        private readonly WeaponService _weaponService;

        public UnitService(WeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        public async Task CreateAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = $@"INSERT INTO `tabletop`.`units` 
                (
                `fraction_id`,
                `defense`,
                `moving`,
                `primary_weapon_id`,
                `secondary_weapon_id`
                )
                VALUES
                (
                @FRACTION_ID,
                @DEFENSE,
                @MOVING,
                @PRIMARY_WEAPON_ID,
                @SECONDARY_WEAPON_ID
                ); {dbController.GetLastIdSql()}";

            input.UnitId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);
        }

        public async Task DeleteAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`units`  WHERE `unit_id` = @UNIT_ID";

            await dbController.QueryAsync(sql, new
            {
                UNIT_ID = input.UnitId,
            }, cancellationToken);
        }

        public async Task DeleteUnitAsync(int userId, int unitId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`user_units` WHERE `user_id` = @USER_ID AND `unit_id` = @UNIT_ID";

            await dbController.QueryAsync(sql, new
            {
                USER_ID = userId,
                UNIT_ID = unitId
            }, cancellationToken);
        }

        public async Task<Unit?> GetAsync(int unitId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM `tabletop`.`units`  WHERE `unit_id` = @UNIT_ID";

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
            string sql = "SELECT * FROM `tabletop`.`units`";

            var list = await dbController.SelectDataAsync<Unit>(sql, cancellationToken: cancellationToken);

            await LoadUnitDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public async Task<List<Unit>> GetAsync(UnitFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT ud.`name`, u.* " +
                "FROM `tabletop`.`unit_description` ud " +
                "INNER JOIN `tabletop`.`units` u " +
                "ON (u.`unit_id` = ud.`unit_id`) " +
                "WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND `code` = @CULTURE");
            sqlBuilder.AppendLine(@$" ORDER BY `name` ASC ");
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
                { "CULTURE", CultureInfo.CurrentCulture.Name }
            };
        }

        public string GetFilterWhere(UnitFilter filter)
        {
            StringBuilder sqlBuilder = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sqlBuilder.AppendLine(@" AND (UPPER(`name`) LIKE @SEARCHPHRASE)");
            }

            string sql = sqlBuilder.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(UnitFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) AS record_count FROM `tabletop`.`unit_description` WHERE 1 = 1 ");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND `code` = @CULTURE");

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public async Task<List<Unit>> GetUserUnitsAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"
                SELECT uu.quantity, u.*
                FROM `tabletop`.`user_units` uu
                INNER JOIN `tabletop`.`units` u ON (u.`unit_id` = uu.`unit_id`)
                WHERE uu.`user_id` = @USER_ID";

            var list = await dbController.SelectDataAsync<Unit>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            return list;
        }

        public async Task<List<Unit>> GetPlayerUnitsAsync(int playerId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"
                SELECT pu.quantity, u.*
                FROM `tabletop`.`player_units` pu
                INNER JOIN `tabletop`.`units` u ON (u.`unit_id` = pu.`unit_id`)
                WHERE pu.`player_id` = @PLAYER_ID";

            var list = await dbController.SelectDataAsync<Unit>(sql, new
            {
                PLAYER_ID = playerId
            }, cancellationToken);

            return list;
        }

        public async Task CreateUserUnitAsync(User user, Unit unit, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`user_units` WHERE `user_id` = @USER_ID AND `unit_id` = @UNIT_ID";
            await dbController.QueryAsync(sql, new
            {
                USER_ID = user.UserId,
                UNIT_ID = unit.UnitId,
            }, cancellationToken);

            sql = @"INSERT INTO `tabletop`.`user_units`
                (
                `user_id`,
                `unit_id`,
                `quantity`
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

        public async Task UpdateAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE `tabletop`.`units` SET
                `fraction_id` = @FRACTION_ID,
                `defense` = @DEFENSE,
                `moving` = @MOVING,
                `primary_weapon_id` = @PRIMARY_WEAPON_ID,
                `secondary_weapon_id` = @SECONDARY_WEAPON_ID
                WHERE `unit_id` = @UNIT_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);
        }

        private static async Task LoadUnitDescriptionsAsync(List<Unit> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Any())
            {
                IEnumerable<int> unitIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM `tabletop`.`unit_description` WHERE `unit_id` IN ({string.Join(",", unitIds)})";
                List<UnitDescription> descriptions = await dbController.SelectDataAsync<UnitDescription>(sql, null, cancellationToken);

                foreach (var unit in list)
                {
                    unit.Description = descriptions.Where(x => x.UnitId == unit.Id).ToList();
                }
            }
        }
    }
}