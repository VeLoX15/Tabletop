using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class WeaponService : IModelService<Weapon, int, WeaponFilter>
    {
        public async Task CreateAsync(Weapon input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = $@"INSERT INTO `tabletop`.`weapons`
                (
                `name`,
                `description`,
                `attack`,
                `quality`,
                `range`,
                `dices`
                )
                VALUES
                (
                @NAME,
                @DESCRIPTION,
                @ATTACK,
                @QUALITY,
                @RANGE,
                @DICES
                ); {dbController.GetLastIdSql()}";

            input.WeaponId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);
        }

        public async Task DeleteAsync(Weapon input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`weapons` WHERE `weapon_id` = @WEAPON_ID";

            await dbController.QueryAsync(sql, new
            {
                WEAPON_ID = input.WeaponId,
            }, cancellationToken);
        }

        public async Task<Weapon?> GetAsync(int? weaponId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM `tabletop`.`weapons` WHERE `weapon_id` = @WEAPON_ID";

            var weapon = await dbController.GetFirstAsync<Weapon>(sql, new
            {
                WEAPON_ID = weaponId
            }, cancellationToken);

            return weapon;
        }

        public async Task<List<Weapon>> GetAsync(WeaponFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT * FROM `tabletop`.`weapons` WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));
            sb.AppendLine(@$"  ORDER BY `name` ASC");
            sb.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sb.ToString();

            List<Weapon> list = await dbController.SelectDataAsync<Weapon>(sql, GetFilterParameter(filter), cancellationToken);

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
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sb.AppendLine(@" AND (UPPER(`name`) LIKE @SEARCHPHRASE)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(WeaponFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sb = new();
            sb.AppendLine("SELECT COUNT(*) FROM `tabletop`.`weapons` WHERE 1 = 1");
            sb.AppendLine(GetFilterWhere(filter));

            string sql = sb.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public static async Task<List<Weapon>> GetAllAsync(IDbController dbController)
        {
            string sql = "SELECT * FROM `tabletop`.`weapons`";

            var list = await dbController.SelectDataAsync<Weapon>(sql);

            return list;
        }

        public async Task UpdateAsync(Weapon input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"UPDATE `tabletop`.`weapons` SET
                `name` = @NAME,
                `description` = @DESCRIPTION,
                `attack` = @ATTACK,
                `quality` = @QUALITY,
                `range` = @RANGE,
                `dices` = @DICES
                WHERE `weapon_id` = @WEAPON_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);
        }

        public Task<Weapon?> GetAsync(int identifier, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
