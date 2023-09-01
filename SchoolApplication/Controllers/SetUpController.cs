using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SchoolApplication.Models.ViewModels;
using SchoolApplication.Models;

namespace SchoolApplication.Controllers
{
    [AllowAnonymous]
    public class SetUpController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public SetUpController(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }


        public IActionResult Index(string token)
        {
            
            var setupToken = _configuration["SetUpToken"];

            if (token != setupToken) 
            {
                return RedirectToAction("Index", "Home");
            } 
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegistererSignUpViewModel model) 
        {
            
            if (ModelState.IsValid) 
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View(model);
                }
                var user = new Registerer
                {
                    Name = model.UserName,
                    UserName = model.Email,
                    Email = model.Email,
                    Phone = model.Phone
                };

                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded) 
                {
                    TempData["success"] = "Registerer successfully created.";
                    await _userManager.AddToRoleAsync(user, "Registerer");
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors) 
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
    }
}
