using DbController;
using DbController.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Web;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel(UserService userService) : PageModel
    {
        [BindProperty]
        public LoginInput Input { get; set; } = new LoginInput();
        public string? ReturnUrl { get; set; }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = GetReturnUrl(returnUrl);

            try
            {
                // Clear the existing external cookie
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }

        }


        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl = GetReturnUrl(returnUrl);

            if (ModelState.IsValid)
            {

                // Erst prüfen wir gegen die Datenbank
                IDbController dbController = new SqlController(AppdataService.ConnectionString);
                User? user = await UserService.GetAsync(Input.Username, dbController);

                // Lokale Konten müssen als ersten geprüft werden.
                if (user is not null)
                {
                    PasswordHasher<User> hasher = new();
                    _ = hasher.HashPassword(user, Input.Password + user.Salt);

                    PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.Password, Input.Password + user.Salt);
                    // Das Handling läuft später auf Basis des Objektes ab.
                    if (result is PasswordVerificationResult.Failed)
                    {
                        user = null;
                    }
                }

                // Wenn wir ein Mitarbeiter Objekt haben, dann können wir uns einloggen, ansonsten ist irgendwas schief gelaufen
                if (user is not null)
                {
                    var claims = new List<Claim>
                    {
                        new("userId", user.UserId.ToString()),
                    };

                    foreach (var permission in user.Permissions)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, permission.Identifier));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = Input.RememberMe,
                        RedirectUri = returnUrl
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    await userService.UpdateLastLoginAsync(user, dbController);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("login-error", "Username or Password are wrong.");
                }
            }
            return Page();
        }

        private string GetReturnUrl(string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return Url.Content("~/");
            }
            else
            {
                string[] parts = returnUrl.Split('/');

                string url = string.Empty;

                foreach (var item in parts)
                {
                    url += $"/{HttpUtility.UrlEncode(item)}";
                }

                return url.Replace("//", "/");
            }
        }
    }

    public class LoginInput
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Stay logged in")]
        public bool RememberMe { get; set; }
    }
}
