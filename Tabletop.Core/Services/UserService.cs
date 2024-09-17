using DbController;
using System.Text;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class UserService(PermissionService permissionService) : IModelService<User, int, UserFilter>
    {
        private readonly PermissionService _permissionService = permissionService;

        public async Task CreateAsync(User input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            input.LastLogin = DateTime.Now;
            input.RegistrationDate = DateTime.Now;

            cancellationToken.ThrowIfCancellationRequested();
            string sql = $@"INSERT INTO Users
                (
                Username,
                DisplayName,
                Password,
                Salt,
                LastLogin,
                RegistrationDate
                )
                VALUES 
                (
                @USERNAME,
                @DISPLAY_NAME,
                @PASSWORD,
                @SALT,
                @LAST_LOGIN,
                @REGISTRATION_DATE
                ); {dbController.GetLastIdSql()}";

            input.UserId = await dbController.GetFirstAsync<int>(sql, input.GetParameters(), cancellationToken);

            await _permissionService.UpdateUserPermissionsAsync(input, dbController, cancellationToken);
        }

        public async Task DeleteAsync(User input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM Users WHERE UserId = @USER_ID";

            await dbController.QueryAsync(sql, new
            {
                USER_ID = input.UserId
            }, cancellationToken);
        }

        public static async Task DeleteFriendAsync(int userId, int friendId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = "DELETE FROM UserFriends WHERE UserId = @USER_ID AND FriendId = @FRIEND_ID";

            await dbController.QueryAsync(sql, new
            {
                USER_ID = userId,
                FRIEND_ID = friendId
            }, cancellationToken);
        }

        public async Task<User?> GetAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM Users WHERE UserId = @USER_ID";

            var user = await dbController.GetFirstAsync<User>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            if (user is not null)
            {
                user.Permissions = await PermissionService.GetUserPermissionsAsync(user.UserId, dbController, cancellationToken);
            }

            return user;
        }

        public static async Task<User?> GetAsync(string username, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT * FROM Users WHERE UPPER(username) = UPPER(@USERNAME)";

            var user = await dbController.GetFirstAsync<User>(sql, new
            {
                USERNAME = username
            }, cancellationToken);

            if (user is not null)
            {
                user.Permissions = await PermissionService.GetUserPermissionsAsync(user.UserId, dbController, cancellationToken);
            }

            return user;
        }

        public static async Task<User?> GetUserForPlayerAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT UserId, Username, DisplayName, Image FROM Users WHERE UserId = @USER_ID";

            var user = await dbController.GetFirstAsync<User>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            return user;
        }

        public async Task<List<User>> GetAsync(UserFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.Append("SELECT UserId, Username, DisplayName, Description, MainFractionId, LastLogin, RegistrationDate, Image FROM Users WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));
            sqlBuilder.AppendLine(@$"  ORDER BY UserId DESC");
            sqlBuilder.AppendLine(dbController.GetPaginationSyntax(filter.PageNumber, filter.Limit));

            // Zum Debuggen schreiben wir den Wert einmal als Variabel
            string sql = sqlBuilder.ToString();

            List<User> list = await dbController.SelectDataAsync<User>(sql, GetFilterParameter(filter), cancellationToken);

            // Berechtigungen müssen noch geladen werden
            List<Permission> permissions = await PermissionService.GetAllAsync(dbController);

            sql = "SELECT * FROM UserPermissions";
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
        UPPER(DisplayName) LIKE @SEARCHPHRASE
    OR  UPPER(Username) LIKE @SEARCHPHRASE
)");
            }

            string sql = sb.ToString();
            return sql;
        }

        public async Task<int> GetTotalAsync(UserFilter filter, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("SELECT COUNT(*) FROM Users WHERE 1 = 1");
            sqlBuilder.AppendLine(GetFilterWhere(filter));

            string sql = sqlBuilder.ToString();

            int result = await dbController.GetFirstAsync<int>(sql, GetFilterParameter(filter), cancellationToken);

            return result;
        }

        public static async Task<List<User>> GetUserFriendsAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT u.UserId, u.Username, u.DisplayName, u.Image
FROM Users AS u
JOIN UserFriends AS uf ON u.UserId = uf.FriendId
WHERE uf.UserId = @USER_ID";

            var list = await dbController.SelectDataAsync<User>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            return list;
        }

        public static async Task<bool> CheckUserFriendAsync(int userId, int friendId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT 
                CASE 
                    WHEN EXISTS (
                        SELECT 1 
                        FROM UserFriends 
                        WHERE UserId = @USER_ID AND FriendId = @FRIEND_ID
                    ) THEN 'true'
                    ELSE 'false'
                END AS IsFriend;";

            bool isfriend = await dbController.GetFirstAsync<bool>(sql, new
            {
                USER_ID = userId,
                FRIEND_ID = friendId
            }, cancellationToken);

            return isfriend;
        }

        public async Task UpdateAsync(User input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"UPDATE Users SET
Username = @USERNAME,
DisplayName = @DISPLAY_NAME,
Description = @DESCRIPTION,
MainFractionId = @MAIN_FRACTION_ID
WHERE UserId = @USER_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);

            await _permissionService.UpdateUserPermissionsAsync(input, dbController, cancellationToken);
        }

        public async Task UpdateLastLoginAsync(User input, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            input.LastLogin = DateTime.Now;
            string sql = @"UPDATE Users SET
LastLogin = @LAST_LOGIN
WHERE UserId = @USER_ID";

            await dbController.QueryAsync(sql, input.GetParameters(), cancellationToken);
        }

        public static async Task<bool> FirstUserExistsAsync(IDbController dbController)
        {
            string sql = "SELECT * FROM Users";

            var tmp = await dbController.GetFirstAsync<object>(sql);

            return tmp != null;
        }

        public async Task<string> GetUsernameAsync(int userId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"SELECT Username FROM Users WHERE UserId = @USER_ID";

            var user = await dbController.GetFirstAsync<User>(sql, new
            {
                USER_ID = userId
            }, cancellationToken);

            if (user != null)
            {
                return user.Username;
            }
            else
            {
                return string.Empty;
            }
        }

        public static async Task CreateUserFriendAsync(int userId, int friendId, IDbController dbController, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string sql = @"INSERT INTO UserFriends
    (
    UserId,
    FriendId
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
