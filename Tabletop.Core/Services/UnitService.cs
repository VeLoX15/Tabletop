using DbController;
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
                //unit.PrimaryWeapon = await _weaponService.GetPrimaryWeaponAsync(unit.UnitId, dbController);
                //unit.SecondaryWeapon = await _weaponService.GetSecondaryWeaponAsync(unit.UnitId, dbController);
            }

            return unit;
        }

        public async Task<List<Unit>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM `tabletop`.`units`";

            var list = await dbController.SelectDataAsync<Unit>(sql);

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
        }
    }
}