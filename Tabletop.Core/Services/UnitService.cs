﻿using DbController;
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
                `name`,
                `fraction_id`,
                `description`,
                `mechanic`,
                `defense`,
                `moving`,
                `primary_weapon_id`,
                `secondary_weapon_id`
                )
                VALUES
                (
                @NAME,
                @FRACTION_ID,
                @DESCRIPTION,
                @MECHANIC,
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

            if (unit != null)
            {
                unit.PrimaryWeapon = await _weaponService.GetAsync(unit.PrimaryWeaponId, dbController, cancellationToken) ?? new();
                unit.SecondaryWeapon = await _weaponService.GetAsync(unit.SecondaryWeaponId, dbController, cancellationToken) ?? new();
            }

            return unit;
        }

        public static async Task<List<Unit>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "SELECT * FROM `tabletop`.`units`";

            var list = await dbController.SelectDataAsync<Unit>(sql, cancellationToken: cancellationToken);

            if (list.Any())
            {
                IEnumerable<int> weaponIds = list.Select(x => x.PrimaryWeaponId ?? 0);
                sql = $"SELECT * FROM `tabletop`.`weapons` WHERE `weapon_id` IN ({string.Join(",", weaponIds)})";
                List<Weapon> weapons = await dbController.SelectDataAsync<Weapon>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.PrimaryWeapon = weapons.FirstOrDefault(x => x.WeaponId == item.PrimaryWeaponId) ?? new();
                }
            }

            if (list.Any())
            {
                IEnumerable<int> weaponIds = list.Select(x => x.SecondaryWeaponId ?? 0);
                sql = $"SELECT * FROM `tabletop`.`weapons` WHERE `weapon_id` IN ({string.Join(",", weaponIds)})";
                List<Weapon> weapons = await dbController.SelectDataAsync<Weapon>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.SecondaryWeapon = weapons.FirstOrDefault(x => x.WeaponId == item.SecondaryWeaponId) ?? new();
                }
            }

            if (list.Any())
            {
                IEnumerable<int> fractionIds = list.Select(x => x.FractionId);
                sql = $"SELECT `name` FROM `tabletop`.`fractions` WHERE `fraction_id` IN ({string.Join(",", fractionIds)})";
                List<Fraction> fractions = await dbController.SelectDataAsync<Fraction>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.Fraction = fractions.FirstOrDefault(x => x.FractionId == item.FractionId) ?? new();
                }
            }

            return list;
        }

        public async Task<List<Unit>> GetAsync(UnitFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT * FROM `tabletop`.`units`  WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$"  ORDER BY `name` ASC");
            sb.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sb.ToString();

            List<Unit> list = await dbController.SelectDataAsync<Unit>(sql, GetFilterParameter(filter), cancellationToken);

            if (list.Any())
            {
                IEnumerable<int> weaponIds = list.Select(x => x.PrimaryWeaponId ?? 0);
                sql = $"SELECT * FROM `tabletop`.`weapons` WHERE `weapon_id` IN ({string.Join(",", weaponIds)})";
                List<Weapon> weapons = await dbController.SelectDataAsync<Weapon>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.PrimaryWeapon = weapons.FirstOrDefault(x => x.WeaponId == item.PrimaryWeaponId) ?? new();
                }
            }

            if (list.Any())
            {
                IEnumerable<int> weaponIds = list.Select(x => x.SecondaryWeaponId ?? 0);
                sql = $"SELECT * FROM `tabletop`.`weapons` WHERE `weapon_id` IN ({string.Join(",", weaponIds)})";
                List<Weapon> weapons = await dbController.SelectDataAsync<Weapon>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.SecondaryWeapon = weapons.FirstOrDefault(x => x.WeaponId == item.SecondaryWeaponId) ?? new();
                }
            }

            if (list.Any())
            {
                IEnumerable<int> fractionIds = list.Select(x => x.FractionId);
                sql = $"SELECT `name` FROM `tabletop`.`fractions` WHERE `fraction_id` IN ({string.Join(",", fractionIds)})";
                List<Fraction> fractions = await dbController.SelectDataAsync<Fraction>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.Fraction = fractions.FirstOrDefault(x => x.FractionId == item.FractionId) ?? new();
                }
            }

            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(UnitFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" }
            };
        }

        public string GetFilterWhere(UnitFilter filter)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sb.AppendLine(@" AND (UPPER(`name`) LIKE @SEARCHPHRASE)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(UnitFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM `tabletop`.`units`  WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

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
                `name` = @NAME,
                `fraction_id` = @FRACTION_ID,
                `description` = @DESCRIPTION,
                `mechanic` = @MECHANIC,
                `defense` = @DEFENSE,
                `moving` = @MOVING,
                `primary_weapon_id` = @PRIMARY_WEAPON_ID,
                `secondary_weapon_id` = @SECONDARY_WEAPON_ID
                WHERE `unit_id` = @UNIT_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);


        }
    }
}