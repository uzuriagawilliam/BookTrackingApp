using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookTrackingApp.Controllers
{
    public class AuthenticationController : Controller
    {
        [Authorize]
        public async Task LogOut()
        {
            //Log out from Book Tracking App
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Log out from OpenId
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
