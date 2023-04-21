using DbController;
using System.Text;
using Tabletop.Filters;
using Tabletop.Interfaces;
using Tabletop.Models;

namespace Tabletop.Services
{
    public class WeaponService : IModelService<Weapon, int, WeaponFilter>
    {
        public async Task CreateAsync(Weapon input, IDbController dbController)
        {
            string sql = $@"INSERT INTO weapons 
(
weapon_id,
name,
description,
attack,
quality,
range,
dices
)
VALUES
(
@WEAPON_ID,
@NAME,
@DESCRIPTION,
@ATTACK,
@QUALITY,
@RANGE,
@DICES
); {dbController.GetLastIdSql()}";

            input.WeaponId = await dbController.GetFirstAsync<int>(sql, input.GetParameters());
        }

        public async Task DeleteAsync(Weapon input, IDbController dbController)
        {
            string sql = "DELETE FROM weapons WHERE weapon_id = @WEAPON_ID";

            await dbController.QueryAsync(sql, new
            {
                WEAPONS_ID = input.WeaponId,
            });
        }

        public async Task<Weapon?> GetAsync(int weaponId, IDbController dbController)
        {
            string sql = @"SELECT * FROM weapons WHERE weapon_id = @WEAPON_ID";

            var weapon = await dbController.GetFirstAsync<Weapon>(sql, new
            {
                WEAPON_ID = weaponId
            });

            return weapon;
        }

        public async Task<List<Weapon>> GetAsync(WeaponFilter filter, IDbController dbController)
        {
            StringBuilder sb = new();
            sb.AppendLine("SELECT * FROM weapons WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$"  ORDER BY weapon_id DESC");
            sb.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sb.ToString();

            List<Weapon> list = await dbController.SelectDataAsync<Weapon>(sql, GetFilterParameter(filter));

            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(WeaponFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" }
            };
        }

        public string GetFilterWhere(WeaponFilter filter)
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

        public async Task<int> GetTotalAsync(WeaponFilter filter, IDbController dbController)
        {
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM weapons WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter));

            return result;
        }

        public async Task UpdateAsync(Weapon input, IDbController dbController)
        {
            string sql = @"UPDATE weapons SET
name = @NAME,
description = @DESCRIPTION,
attack = @ATTACK,
quality = @QUALITY,
range = @RANGE,
dices = @DICES
WHERE weapon_id = @WEAPON_ID";

            await dbController.QueryAsync(sql, input.GetParameters());
        }

        public Task UpdateAsync(Weapon input, Weapon oldInputToCompare, IDbController dbController)
        {
            throw new NotImplementedException();
        }
    }
}
