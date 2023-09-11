using DbController;
using System.Threading;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class ClassService : IModelService<Class, int>
    {
        public Task CreateAsync(Class input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Class input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public static async Task<List<Class>> GetAllAsync(IDbController dbController, CancellationToken cancellationToken = default)
        {
            string sql = "SELECT * FROM `tabletop`.`classes`";

            var list = await dbController.SelectDataAsync<Class>(sql, cancellationToken: cancellationToken);
            await LoadClassDescriptionsAsync(list, dbController, cancellationToken);
            return list;
        }

        public Task<Class?> GetAsync(int identifier, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private static async Task LoadClassDescriptionsAsync(List<Class> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Any())
            {
                IEnumerable<int> classIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM `tabletop`.`class_description` WHERE `class_id` IN ({string.Join(",", classIds)})";
                List<ClassDescription> descriptions = await dbController.SelectDataAsync<ClassDescription>(sql, null, cancellationToken);

                foreach (var item in list)
                {
                    item.Description = descriptions.Where(x => x.ClassId == item.Id).ToList();
                }
            }
        }

        public Task UpdateAsync(Class input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
