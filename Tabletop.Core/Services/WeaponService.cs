using DbController;
using System.Globalization;
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
            string sql = $@"INSERT INTO Weapons
                (
                Attack,
                Quality,
                Range,
                Dices
                )
                VALUES
                (
                @ATTACK,
                @QUALITY,
                @RANGE,
                @DICES
                ); {dbController.GetLastIdSql()}";

            input.WeaponId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"INSERT INTO WeaponDescription
                    (
                    WeaponId,
                    Code,
                    Name,
                    Description
                    )
                    VALUES
                    (
                    @WEAPON_ID,
                    @CODE,
                    @NAME,
                    @DESCRIPTION
                    )";

                var parameters = new
                {
                    WEAPON_ID = input.WeaponId,
                    CODE = description.Code,
                    NAME = description.Name,
                    DESCRIPTION = description.Description
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        public async Task DeleteAsync(Weapon input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM Weapons WHERE WeaponId = @WEAPON_ID";

            await dbController.QueryAsync(sql, new
            {
                WEAPON_ID = input.WeaponId
            }, cancellationToken);
        }

        public async Task<Weapon?> GetAsync(int weaponId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM Weapons WHERE WeaponId = @WEAPON_ID";

            var weapon = await dbController.GetFirstAsync<Weapon>(sql, new
            {
                WEAPON_ID = weaponId
            }, cancellationToken);

            if(weapon != null)
            {
                await LoadWeaponDescriptionAsync(weapon, dbController, cancellationToken);
            }
            return weapon;
        }

        public async Task<List<Weapon>> GetAsync(WeaponFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT wd.Name, w.* " +
                "FROM WeaponDescription wd " +
                "INNER JOIN Weapons w " +
                "ON (w.WeaponId = wd.WeaponId) " +
                "WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");
            sqlBuilder.AppendLine(@$" ORDER BY Name ASC ");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            string sql = sqlBuilder.ToString();

            List<Weapon> list = await dbController.SelectDataAsync<Weapon>(sql, GetFilterParameter(filter), cancellationToken);

            await LoadWeaponDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(WeaponFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" },
                { "CULTURE", CultureInfo.CurrentCulture.Name }
            };
        }

        public string GetFilterWhere(WeaponFilter filter)
        {
            StringBuilder sqlBuilder = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sqlBuilder.AppendLine(@" AND (UPPER(Name) LIKE @SEARCHPHRASE)");
            }

            string sql = sqlBuilder.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(WeaponFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) AS record_count FROM WeaponDescription WHERE 1 = 1 ");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@" AND Code = @CULTURE");

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public static async Task<List<Weapon>> GetAllAsync(IDbController dbController)
        {
            string sql = "SELECT * FROM Weapons";

            var list = await dbController.SelectDataAsync<Weapon>(sql);
            await LoadWeaponDescriptionsAsync(list, dbController);
            return list;
        }

        public async Task UpdateAsync(Weapon input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"UPDATE Weapons SET
                Attack = @ATTACK,
                Quality = @QUALITY,
                Range = @RANGE,
                Dices = @DICES
                WHERE WeaponId = @WEAPON_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            foreach (var description in input.Description)
            {
                sql = @"UPDATE WeaponDescription SET
                    Name = @NAME,
                    Description = @DESCRIPTION
                    WHERE WeaponId = @WEAPON_ID AND Code = @CODE";

                var parameters = new
                {
                    WEAPON_ID = input.WeaponId,
                    CODE = description.Code,
                    NAME = description.Name,
                    DESCRIPTION = description.Description
                };

                await dbController.QueryAsync(sql, parameters, cancellationToken);
            }
        }

        private static async Task LoadWeaponDescriptionsAsync(List<Weapon> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Count != 0)
            {
                IEnumerable<int> weaponIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM WeaponDescription WHERE WeaponId IN ({string.Join(",", weaponIds)})";
                List<WeaponDescription> descriptions = await dbController.SelectDataAsync<WeaponDescription>(sql, null, cancellationToken);

                foreach (var weapon in list)
                {
                    weapon.Description = descriptions.Where(x => x.WeaponId == weapon.Id).ToList();
                }
            }
        }

        private static async Task LoadWeaponDescriptionAsync(Weapon weapon, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();


            string sql = $"SELECT * FROM WeaponDescription WHERE WeaponId IN @WEAPON_ID";
            weapon.Description = await dbController.SelectDataAsync<WeaponDescription>(sql, new
            {
                WEAPON_ID = weapon.WeaponId
            }, cancellationToken);
        }
    }
}