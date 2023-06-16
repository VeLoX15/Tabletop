using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class UnitService : IModelService<Unit, int, UnitFilter>
    {
        private readonly FractionService _fractionService;
        private readonly WeaponService _weaponService;

        public UnitService(FractionService fractionService, WeaponService weaponService)
        {
            _fractionService = fractionService;
            _weaponService = weaponService;
        }

        public async Task CreateAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = $@"INSERT INTO `tabletop`.`units` 
                (
                `name`,
                `fraction`,
                `description`,
                `defense`,
                `moving`
                )
                VALUES
                (
                @NAME,
                @FRACTION,
                @DESCRIPTION,
                @DEFENSE,
                @MOVING
                ); {dbController.GetLastIdSql()}";

            input.UnitId = await dbController.GetFirstAsync<int>(sql, input.GetParameters());
        }

        public async Task DeleteAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "DELETE FROM `tabletop`.`units`  WHERE `unit_id` = @UNIT_ID";

            await dbController.QueryAsync(sql, new
            {
                UNIT_ID = input.UnitId,
            });
        }

        public async Task<Unit?> GetAsync(int unitId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"SELECT * FROM `tabletop`.`units`  WHERE `unit_id` = @UNIT_ID";

            var unit = await dbController.GetFirstAsync<Unit>(sql, new
            {
                UNIT_ID = unitId
            });

            if (unit is not null)
            {
                unit.Fraction = await _fractionService.GetAsync(unit.FractionId, dbController) ?? new();
                unit.PrimaryWeapon = await _weaponService.GetAsync(unit.PrimaryWeaponId, dbController) ?? new();
                unit.SecondaryWeapon = await _weaponService.GetAsync(unit.SecondaryWeaponId, dbController) ?? new();
            }

            return unit;
        }

        public static async Task<List<Unit>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM `tabletop`.`units`";

            var list = await dbController.SelectDataAsync<Unit>(sql);

            if (list.Any())
            {
                IEnumerable<int> fractionIds = list.Select(x => x.FractionId);
                sql = $"SELECT * FROM `tabletop`.`fractions` WHERE `fraction_id` IN ({string.Join(",", fractionIds)})";
                List<Fraction> fractions = await dbController.SelectDataAsync<Fraction>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.Fraction = fractions.FirstOrDefault(x => x.FractionId == item.FractionId) ?? new();
                }
            }

            if (list.Any())
            {
                IEnumerable<int> weaponIds = list.Select(x => x.PrimaryWeaponId);
                sql = $"SELECT * FROM `tabletop`.`weapons` WHERE `weapon_id` IN ({string.Join(",", weaponIds)})";
                List<Weapon> weapons = await dbController.SelectDataAsync<Weapon>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.PrimaryWeapon = weapons.FirstOrDefault(x => x.WeaponId == item.PrimaryWeaponId) ?? new();
                }
            }

            if (list.Any())
            {
                IEnumerable<int> weaponIds = list.Select(x => x.SecondaryWeaponId);
                sql = $"SELECT * FROM `tabletop`.`weapons` WHERE `weapon_id` IN ({string.Join(",", weaponIds)})";
                List<Weapon> weapons = await dbController.SelectDataAsync<Weapon>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.SecondaryWeapon = weapons.FirstOrDefault(x => x.WeaponId == item.SecondaryWeaponId) ?? new();
                }
            }

            return list;
        }

        public async Task<List<Unit>> GetAsync(UnitFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            StringBuilder sb = new();
            sb.AppendLine("SELECT * FROM `tabletop`.`units`  WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$"  ORDER BY `unit_id` DESC");
            sb.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sb.ToString();

            List<Unit> list = await dbController.SelectDataAsync<Unit>(sql, GetFilterParameter(filter));

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
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sb.AppendLine(@" AND (UPPER(`name`) LIKE @SEARCHPHRASE)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(UnitFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM `tabletop`.`units`  WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter));

            return result;
        }

        public async Task UpdateAsync(Unit input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = @"UPDATE `tabletop`.`units` SET
                `name` = @NAME,
                `fraction` = @FRACTION,
                `description` = @DESCRIPTION,
                `defense` = @DEFENSE,
                `moving` = @MOVING
                WHERE `unit_id` = @UNIT_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            await _weaponService.UpdateAsync(input.PrimaryWeapon, dbController, cancellationToken);
            await _weaponService.UpdateAsync(input.SecondaryWeapon, dbController, cancellationToken);
        }
    }
}