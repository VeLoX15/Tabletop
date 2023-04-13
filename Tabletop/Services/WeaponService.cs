﻿using DbController;
using Tabletop.Interfaces;
using Tabletop.Models;

namespace Tabletop.Core.Services.Services
{
    public class WeaponService : IModelService<Weapon, int>
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
