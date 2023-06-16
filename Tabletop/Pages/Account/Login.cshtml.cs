using DbController;
using DbController.MySql;
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
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;

        [BindProperty]
        public LoginInput Input { get; set; } = new LoginInput();
        public string? ReturnUrl { get; set; }

        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

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

                // Erst pr�fen wir gegen die Datenbank
                IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                User? user = AppdataService.IsLocalLoginEnabled ? await _userService.GetAsync(Input.Username, dbController) : null;

                // Lokale Konten m�ssen als ersten gepr�ft werden.
                if (user is not null)
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();

                    string passwordHashed = hasher.HashPassword(user, Input.Password + user.Salt);

                    PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.Password, Input.Password + user.Salt);
                    // Das Handling l�uft sp�ter auf Basis des Objektes ab.
                    if (result is PasswordVerificationResult.Failed)
                    {
                        user = null;
                    }
                }

                // Wenn wir ein Mitarbeiter Objekt haben, dann k�nnen wir uns einloggen, ansonsten ist irgendwas schief gelaufen
                if (user is not null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("userId", user.UserId.ToString()),
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
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    if (!AppdataService.IsLocalLoginEnabled)
                    {
                        ModelState.AddModelError("login-error", "Es wurde kein Provider zum einloggen gefunden. Bitte wenden Sie sich an Ihren Administrator.");
                    }
                    else
                    {
                        ModelState.AddModelError("login-error", "Username oder Passwort ist falsch.");
                    }
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
        public string Username { get; set; } = String.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;
        [Display(Name = "Eingeloggt bleiben")]
        public bool RememberMe { get; set; }
    }
}