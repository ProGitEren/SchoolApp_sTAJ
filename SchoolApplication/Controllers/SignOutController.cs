using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SchoolApplication.Controllers
{
    public class SignOutController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public SignOutController(SignInManager <IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Get
        public async Task<IActionResult> Logout()
        {
            // Sign out the user from the authentication system
            await _signInManager.SignOutAsync();

            // Sign out from all external authentication providers (if any)
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            TempData["success"] = "You successfully logged out.";
            // Redirect to the home page or any other desired location
            return RedirectToAction("Index", "Home");
        }
    }
}
