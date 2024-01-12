using DbController;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class AbilityService : IModelService<Ability, int>
    {
        public Task CreateAsync(Ability input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Ability input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public static async Task<List<Ability>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM `tabletop`.`abilities`";

            var list = await dbController.SelectDataAsync<Ability>(sql, cancellationToken: cancellationToken);
            await LoadClassDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public Task<Ability?> GetAsync(int identifier, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private static async Task LoadClassDescriptionsAsync(List<Ability> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Any())
            {
                IEnumerable<int> abilityIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM `tabletop`.`ability_description` WHERE `ability_id` IN ({string.Join(",", abilityIds)})";
                List<AbilityDescription> descriptions = await dbController.SelectDataAsync<AbilityDescription>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.Description = descriptions.Where(x => x.AbilityId == item.Id).ToList();
                }
            }
        }

        public Task UpdateAsync(Ability input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
