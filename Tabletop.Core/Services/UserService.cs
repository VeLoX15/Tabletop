﻿using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class UserService : IModelService<User, int, UserFilter>
    {
        private readonly PermissionService _permissionService;

        public UserService(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        public async Task CreateAsync(User input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = $@"INSERT INTO `tabletop`.`users`
    (
    `username`,
    `display_name`,
    `password`,
    `salt`,
    `last_login`,
    `image`
    )
    VALUES 
    (
    @USERNAME,
    @DISPLAY_NAME,
    @PASSWORD,
    @SALT,
    @LAST_LOGIN,
    @IMAGE
    ); {dbController.GetLastIdSql()}";

            input.UserId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            await _permissionService.UpdateUserPermissionsAsync(input, dbController, cancellationToken);
        }

        public async Task DeleteAsync(User input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`users``WHERE `user_id` = @USER_ID";

            await dbController.QueryAsync(sql, new
            {
                USER_ID = input.UserId,
            }, cancellationToken);
        }

        public async Task DeleteFriendAsync(int userId, int friendId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM `tabletop`.`user_friends` WHERE `user_id` = @USER_ID AND `friend_id` = @FRIEND_ID";

            await dbController.QueryAsync(sql, new
            {
                USER_ID = userId,
                FRIEND_ID = friendId
            }, cancellationToken);
        }

        public async Task<User?> GetAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM `tabletop`.`users` WHERE `user_id` = @USER_ID";

            var user = await dbController.GetFirstAsync<User>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            if (user is not null)
            {
                user.Permissions = await _permissionService.GetUserPermissionsAsync(user.UserId, dbController, cancellationToken);
            }

            return user;
        }

        public async Task<User?> GetAsync(string username, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM `tabletop`.`users` WHERE UPPER(username) = UPPER(@USERNAME)";

            var user = await dbController.GetFirstAsync<User>(sql, new
            {
                USERNAME = username
            }, cancellationToken);

            if (user is not null)
            {
                user.Permissions = await _permissionService.GetUserPermissionsAsync(user.UserId, dbController, cancellationToken);
            }

            return user;
        }

        public async Task<List<User>> GetAsync(UserFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.Append("SELECT * FROM `tabletop`.`users` WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@$"  ORDER BY `user_id` DESC");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            // Zum Debuggen schreiben wir den Wert einmal als Variabel
            string sql = sqlBuilder.ToString();

            List<User> list = await dbController.SelectDataAsync<User>(sql, GetFilterParameter(filter), cancellationToken);

            // Berechtigungen müssen noch geladen werden
            List<Permission> permissions = await PermissionService.GetAllAsync(dbController);

            sql = "SELECT * FROM `tabletop`.`user_permissions`";
            List<UserPermission> user_permissions = await dbController.SelectDataAsync<UserPermission>(sql, null, cancellationToken);

            foreach (var user in list)
            {
                List<UserPermission> permissions_for_user = user_permissions.Where(x => x.UserId == user.UserId).ToList();
                List<int> permission_ids = permissions_for_user.Select(x => x.PermissionId).ToList();

                user.Permissions = permissions.Where(x => permission_ids.Contains(x.PermissionId)).ToList();
            }

            return list;
        }

        public Dictionary<string, object?> GetFilterParameter(UserFilter filter)
        {
            return new Dictionary<string, object?>
            {
                { "SEARCHPHRASE", $"%{filter.SearchPhrase}%" }
            };
        }

        public string GetFilterWhere(UserFilter filter)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                sb.AppendLine(@" AND 
(
        UPPER(display_name) LIKE @SEARCHPHRASE
    OR  UPPER(username) LIKE @SEARCHPHRASE
)");
            }

            //if (filter.BlockedIds.Any())
            //{
            //    sb.AppendLine($" AND user_id NOT IN ({string.Join(",", filter.BlockedIds)})");
            //}


            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(UserFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) FROM `tabletop`.`users` WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public async Task<List<User>> GetUserFriendsAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT u.user_id, u.username, u.display_name, u.image
FROM tabletop.users AS u
JOIN tabletop.user_friends AS uf ON u.user_id = uf.friend_id
WHERE uf.user_id = @USER_ID";

            var list = await dbController.SelectDataAsync<User>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            return list;
        }

        public async Task UpdateAsync(User input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"UPDATE `tabletop`.`users` SET
`username` = @USERNAME,
`display_name` = @DISPLAY_NAME
WHERE user_id = @USER_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            await _permissionService.UpdateUserPermissionsAsync(input, dbController, cancellationToken);
        }
        public static async Task<bool> FirstUserExistsAsync(IDbController dbController)
        {
            string sql = "SELECT * FROM `tabletop`.`users`";

            var tmp = await dbController.GetFirstAsync<object>(sql);

            return tmp != null;
        }

        public async Task CreateUserFriendAsync(int userId, int friendId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"INSERT INTO `tabletop`.`user_friends`
    (
    user_id,
    friend_id
    )
    VALUES
    (
    @USER_ID,
    @FRIEND_ID
    )";

            await dbController.QueryAsync(sql, new
            {
                USER_ID = userId,
                FRIEND_ID = friendId
            }, cancellationToken);
        }
    }
}
