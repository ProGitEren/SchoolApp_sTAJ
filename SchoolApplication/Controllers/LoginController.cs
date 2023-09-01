using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolApplication.Models.ViewModels;

namespace SchoolApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
       

        public LoginController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;

            _userManager = userManager;

        }
        public IActionResult StudentLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentLogin(StudentLoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    //get the role of the
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains("Student"))
                    {
                        ModelState.AddModelError("", "Only students are allowed to log in here. Student and Teacher logins are seperate and can be seen from the navigation bar.");
                        return View(model);
                    }
                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        TempData["success"] = "You are successfully logged in with a Student role.";
                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Account locked out. Please try again later.");
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        ModelState.AddModelError("", "Two-factor authentication is required for this account.");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "Login is not allowed for this account.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
            }

            return View(model);
        }
        public IActionResult TeacherLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherLogin(TeacherLoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    //get the role of the
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains("Teacher"))
                    {
                        ModelState.AddModelError("", "Only teachers are allowed to log in here. Student and Teacher logins are seperate and can be seen from the navigation bar.");
                        return View(model);
                    }
                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        TempData["success"] = "You are successfully logged in with a Teacher role.";
                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Account locked out. Please try again later.");
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        ModelState.AddModelError("", "Two-factor authentication is required for this account.");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "Login is not allowed for this account.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult RegistererLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistererLogin(RegistererLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                //check if exist in database
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    //get the role of the
                    var roles = await _userManager.GetRolesAsync(user);

                    if (!roles.Contains("Registerer"))
                    {
                        ModelState.AddModelError("", "Only registerers are allowed to log in here. Student and Teacher logins are seperate and can be seen from the navigation bar.");
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        TempData["success"] = "You are successfully logged in with a Registerer role.";
                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Account locked out. Please try again later.");
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        ModelState.AddModelError("", "Two-factor authentication is required for this account.");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "Login is not allowed for this account.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        
        }


    }
}
