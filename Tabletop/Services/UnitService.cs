using DbController;
using Tabletop.Interfaces;
using Tabletop.Models;

namespace Tabletop.Services
{
    public class UnitService : IModelService<Unit, int>
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
)
VALUES
(
@UNIT_ID,
@NAME,
@FRACTION,
@DESCRIPTION,
@DEFENSE,
@MOVING
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

        public async Task UpdateAsync(Unit input, IDbController dbController)
        {
            string sql = @"UPDATE units SET
name = @NAME,
fraction = @FRACTION,
description = @DESCRIPTION,
defense = @DEFENSE,
moving = MOVING
WHERE unit_id = @UNIT_ID";

            await dbController.QueryAsync(sql, input.GetParameters());
        }

        public Task UpdateAsync(Unit input, Unit oldInputToCompare, IDbController dbController)
        {
            throw new NotImplementedException();
        }
    }
}
