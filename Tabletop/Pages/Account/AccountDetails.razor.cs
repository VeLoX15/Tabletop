using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Tabletop.Core.Models;

namespace Tabletop.Pages.Account
{
    public partial class AccountDetails
    {
#nullable disable
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
#nullable enable

        public User? CurrentUser { get; set; }
        private ClaimsPrincipal? _user;
        protected override async Task OnParametersSetAsync()
        {
            if (AuthState is not null)
            {
                _user = (await AuthState).User;

                CurrentUser = await authService.GetUserAsync();

                if (CurrentUser?.Image != null)
                {
                    string base64String = Convert.ToBase64String(CurrentUser.Image);
                    CurrentUser.ConvertedImage = $"data:image/png;base64,{base64String}";
                }
            }
        }
    }
}