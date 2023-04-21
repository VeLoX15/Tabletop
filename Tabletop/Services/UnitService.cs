using DbController;
using System.Text;
using Tabletop.Filters;
using Tabletop.Interfaces;
using Tabletop.Models;

namespace Tabletop.Services
{
    public class UnitService : IModelService<Unit, int, UnitFilter>
    {
        public async Task CreateAsync(Unit input, IDbController dbController)
        {
            string sql = $@"INSERT INTO units 
(
unit_id,
name,
fraction,
description,
defense,
moving,
primary_weapon_id,
secondary_weapon_id
)
VALUES
(
@UNIT_ID,
@NAME,
@FRACTION,
@DESCRIPTION,
@DEFENSE,
@MOVING
@PRIMARY_WEAPON_ID,
@SECONDARY_WEAPON_ID
); {dbController.GetLastIdSql()}";

            input.UnitId = await dbController.GetFirstAsync<int>(sql, input.GetParameters());
        }

        public async Task DeleteAsync(Unit input, IDbController dbController)
        {
            string sql = "DELETE FROM units WHERE unit_id = @UNIT_ID";

            await dbController.QueryAsync(sql, new
            {
                UNIT_ID = input.UnitId,
            });
        }

        public async Task<Unit?> GetAsync(int unitId, IDbController dbController)
        {
            string sql = @"SELECT * FROM units WHERE unit_id = @UNIT_ID";

            var unit = await dbController.GetFirstAsync<Unit>(sql, new
            {
                UNIT_ID = unitId
            });

            return unit;
        }

        public async Task<List<Unit>> GetAsync(UnitFilter filter, IDbController dbController)
        {
            StringBuilder sb = new();
            sb.AppendLine("SELECT * FROM units WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$"  ORDER BY unit_id DESC");
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
                sb.AppendLine(@" AND 
(
    UPPER(name) LIKE @SEARCHPHRASE
)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(UnitFilter filter, IDbController dbController)
        {
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM units WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter));

            return result;
        }

        public async Task UpdateAsync(Unit input, IDbController dbController)
        {
            string sql = @"UPDATE units SET
name = @NAME,
fraction = @FRACTION,
description = @DESCRIPTION,
defense = @DEFENSE,
moving = @MOVING,
primary_weapon_id = @PRIMARY_WEAPON_ID,
secondary_weapon_id = @SECONDARY_WEAPON_ID
WHERE unit_id = @UNIT_ID";

            await dbController.QueryAsync(sql, input.GetParameters());
        }

        public Task UpdateAsync(Unit input, Unit oldInputToCompare, IDbController dbController)
        {
            throw new NotImplementedException();
        }
    }
}
