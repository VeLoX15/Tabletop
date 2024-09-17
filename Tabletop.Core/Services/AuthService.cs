using DbController;
using DbController.SqlServer;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Tabletop.Core.Models;

namespace Tabletop.Core.Services
{
    public class AuthService(AuthenticationStateProvider authenticationStateProvider, UserService userService)
    {

        /// <summary>
        /// Converts the active claims into a <see cref="User"/> object
        /// </summary>
        /// <returns></returns>
        public async Task<User?> GetUserAsync(IDbController? dbController = null)
        {

            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                Claim? claim = user.FindFirst("UserId");
                if (claim is null)
                {
                    return null;
                }

                var userId = Convert.ToInt32(claim.Value);

                bool shouldDispose = dbController is null;


                dbController ??= new SqlController(AppdataService.ConnectionString);

                var result = await userService.GetAsync(userId, dbController);

                if (shouldDispose)
                {
                    dbController.Dispose();
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Checks if the currently logged in user as a specific role within it's claims.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> HasRole(string roleName)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return user.IsInRole(roleName);
        }
    }
}
