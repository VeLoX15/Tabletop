using DbController;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class PermissionService
    {
        public static async Task<List<Permission>> GetUserPermissionsAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT p.*
                FROM UserPermissions up
                INNER JOIN Permissions p ON (p.PermissionId = up.PermissionId)
                WHERE UserId = @USER_ID";

            var list = await dbController.SelectDataAsync<Permission>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            await LoadPermissionDescriptionsAsync(list, dbController, cancellationToken);

            return list;
        }

        public async Task UpdateUserPermissionsAsync(User user, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // Step 1: Delete all permissions for the user.
            string sql = "DELETE FROM UserPermissions WHERE UserId = @USER_ID";
            await dbController.QueryAsync(sql, new
            {
                USER_ID = user.UserId
            }, cancellationToken);

            // Step 2: Add all permissions from the object back.
            foreach (var permission in user.Permissions)
            {
                sql = @"INSERT INTO UserPermissions
    (
    UserId,
    PermissionId
    )
    VALUES
    (
    @USER_ID,
    @PERMISSION_ID
    )";

                await dbController.QueryAsync(sql, new
                {
                    USER_ID = user.UserId,
                    PERMISSION_ID = permission.PermissionId
                }, cancellationToken);

            }
        }

        public static async Task<List<Permission>> GetAllAsync(IDbController dbController)
        {
            string sql = "SELECT * FROM Permissions";

            var list = await dbController.SelectDataAsync<Permission>(sql);
            await LoadPermissionDescriptionsAsync(list, dbController);
            return list;
        }

        private static async Task LoadPermissionDescriptionsAsync(List<Permission> list, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (list.Count != 0)
            {
                IEnumerable<int> permissionIds = list.Select(x => x.Id);
                string sql = $"SELECT * FROM PermissionDescription WHERE PermissionId IN ({string.Join(",", permissionIds)})";
                List<PermissionDescription> descriptions = await dbController.SelectDataAsync<PermissionDescription>(sql, null, cancellationToken);

                foreach (var permission in list)
                {
                    permission.Description = descriptions.Where(x => x.PermissionId == permission.Id).ToList();
                }
            }
        }
    }
}