using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AngularIdentityAuth.Controllers.Security
{
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl)
        {
            return new ChallengeResult(
                GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(LoginCallback), new { returnUrl })
                });
        }
        
        public async Task<IActionResult> LoginCallback(string returnUrl)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("External");

            if (!authenticateResult?.Succeeded ?? true)
                return BadRequest(); // TODO: Handle this better.

            var claimsIdentity = new ClaimsIdentity("Application");

            if (authenticateResult?.Principal != null)
            {
                var nameIdentifier = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier);
                var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email);

                if (nameIdentifier != null)
                {
                    claimsIdentity.AddClaim(nameIdentifier);
                }

                if (email != null)
                {
                    claimsIdentity.AddClaim(email);
                }

                await HttpContext.SignInAsync(
                    "Application",
                    new ClaimsPrincipal(claimsIdentity));
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("External");

            return Redirect("/Account/Login");
        }

        public IActionResult GetIdentity() 
        {
            return Json(new 
            {
                Success = !string.IsNullOrWhiteSpace(User.FindFirst(ClaimTypes.Email)?.Value),
                Claims = new 
                {
                    Name = User.FindFirst(ClaimTypes.Name)?.Value,
                    Email = User.FindFirst(ClaimTypes.Email)?.Value
                }
            });
        }
    }
}
