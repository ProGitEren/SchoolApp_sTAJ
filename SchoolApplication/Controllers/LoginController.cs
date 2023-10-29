using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolApplication.Expire;
using SchoolApplication.Models.ViewModels;
using System.Globalization;
using System.Text.Json;

namespace SchoolApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;


        //For login attempts and lockout for 2 minutes after 4 trials
       
        


        public LoginController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager, ILogger<LoginController> logger, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;

            _userManager = userManager;

            _logger = logger;

            _roleManager = roleManager;

        }
        public void checkandsetSession()
        {

            var session = HttpContext.Session.GetInt32("Key");
            if (session == null)
            {
                HttpContext.Session.SetInt32("Key", 1);
            }
            else
            {
                var newvalue = (int)session + 1;

                HttpContext.Session.SetInt32("Key", newvalue);
                //var session1 = HttpContext.Session.GetInt32("Key");
            }
        }

        [AllowAnonymous]
        public IActionResult StudentLogin()
        {
            //put this statement since at first page opening error for not have an instance of  logintrial
            var model = new StudentLoginViewModel { logintrial = 5 };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentLogin(StudentLoginViewModel model)
        {

            if (!ModelState.IsValid) { return View(model); }

            var lockoutEndTimeString_2 = HttpContext.Session.GetString("LockoutEndTime");
            if (!string.IsNullOrEmpty(lockoutEndTimeString_2))
            {
                var lockoutEndTime_2 = DateTime.ParseExact(lockoutEndTimeString_2, "HH:mm:ss", CultureInfo.InvariantCulture);
                if (DateTime.UtcNow < lockoutEndTime_2)
                {
                    // User is still locked out
                    ModelState.AddModelError("", $"You are locked out. Please try again later. Time remaining: {lockoutEndTime_2 - DateTime.UtcNow:mm\\:ss} minutes ");
                    return View(model);
                }
                else
                {
                    // Lockout duration has passed; clear the lockout flag
                    HttpContext.Session.SetInt32("Key", 0);
                    HttpContext.Session.Remove("LockoutEndTime");
                }
            }


            if (HttpContext.Session.GetInt32("Key") == null || HttpContext.Session.GetInt32("Key") < 3)
            // Check if the user is locked out
            {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user != null)
                    {
                        //get the role of the
                        var roles = await _userManager.GetRolesAsync(user);
                        if (!roles.Contains("Student"))
                        {
                            checkandsetSession();
                            int session_intermediate = (int) HttpContext.Session.GetInt32("Key");
                            model.logintrial = 4 - session_intermediate;
                            ModelState.AddModelError("", $"Only students are allowed to log in here. Student and Teacher logins are seperate and can be seen from the navigation bar.");     
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
                            checkandsetSession();
                            int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                            model.logintrial = 4 - session_intermediate;
                            ModelState.AddModelError("", "Account locked out. Please try again later.");
                            return View(model);
                        }
                        else if (result.RequiresTwoFactor)
                        {
                            checkandsetSession();
                            int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                            model.logintrial = 4 - session_intermediate;
                            ModelState.AddModelError("", "Two-factor authentication is required for this account.");
                            return View(model);
                        }
                        else if (result.IsNotAllowed)
                        {
                            checkandsetSession();
                            int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                            model.logintrial = 4 - session_intermediate;
                            ModelState.AddModelError("", "Login is not allowed for this account.");
                            return View(model);
                        }
                        else
                        {
                            checkandsetSession();
                            int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                            model.logintrial = 4 - session_intermediate;
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                        }

                    }
                    else
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Invalid username or password.");
                        return View(model);
                    }      
            }

            var lockoutEndTime = DateTime.UtcNow.AddMinutes(2);
            var lockoutEndTimeString = lockoutEndTime.ToString("HH:mm:ss");
            HttpContext.Session.SetString("LockoutEndTime", lockoutEndTimeString);
            ModelState.AddModelError("", "You are locked out for 2 minutes beacuse of several unsuccessfull attemps!!");
            return View(model);

        }

        [AllowAnonymous]
        public IActionResult TeacherLogin()
        {
            TeacherLoginViewModel model = new TeacherLoginViewModel { logintrial = 5 };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherLogin(TeacherLoginViewModel model)
        {

            if (!ModelState.IsValid) { return View(model); }

            var lockoutEndTimeString_2 = HttpContext.Session.GetString("LockoutEndTime");
            if (!string.IsNullOrEmpty(lockoutEndTimeString_2))
            {
                var lockoutEndTime_2 = DateTime.ParseExact(lockoutEndTimeString_2, "HH:mm:ss", CultureInfo.InvariantCulture);
                if (DateTime.UtcNow < lockoutEndTime_2)
                {
                    // User is still locked out
                    ModelState.AddModelError("", $"You are locked out. Please try again later. Time remaining: {lockoutEndTime_2 - DateTime.UtcNow:mm\\:ss} minutes ");
                    return View(model);
                }
                else
                {
                    // Lockout duration has passed; clear the lockout flag
                    HttpContext.Session.SetInt32("Key", 0);
                    HttpContext.Session.Remove("LockoutEndTime");
                }
            }


            if (HttpContext.Session.GetInt32("Key") == null || HttpContext.Session.GetInt32("Key") < 3)
            // Check if the user is locked out
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    //get the role of the
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains("Teacher"))
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", $"Only teachers are allowed to log in here. Student and Teacher logins are seperate and can be seen from the navigation bar.");
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
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Account locked out. Please try again later.");
                        return View(model);
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Two-factor authentication is required for this account.");
                        return View(model);
                    }
                    else if (result.IsNotAllowed)
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Login is not allowed for this account.");
                        return View(model);
                    }
                    else
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }

                }
                else
                {
                    checkandsetSession();
                    int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                    model.logintrial = 4 - session_intermediate;
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
            }

            var lockoutEndTime = DateTime.UtcNow.AddMinutes(2);
            var lockoutEndTimeString = lockoutEndTime.ToString("HH:mm:ss");
            HttpContext.Session.SetString("LockoutEndTime", lockoutEndTimeString);
            ModelState.AddModelError("", "You are locked out for 2 minutes beacuse of several unsuccessfull attemps!!");
            return View(model);

        }

        [AllowAnonymous]
        public IActionResult RegistererLogin()
        {
            RegistererLoginViewModel model = new RegistererLoginViewModel { logintrial = 5 };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistererLogin(RegistererLoginViewModel model)
        {

            if (!ModelState.IsValid) { return View(model); }

            var lockoutEndTimeString_2 = HttpContext.Session.GetString("LockoutEndTime");
            if (!string.IsNullOrEmpty(lockoutEndTimeString_2))
            {
                var lockoutEndTime_2 = DateTime.ParseExact(lockoutEndTimeString_2, "HH:mm:ss", CultureInfo.InvariantCulture);
                if (DateTime.UtcNow < lockoutEndTime_2)
                {
                    // User is still locked out
                    ModelState.AddModelError("", $"You are locked out. Please try again later. Time remaining: {lockoutEndTime_2 - DateTime.UtcNow:mm\\:ss} minutes ");
                    return View(model);
                }
                else
                {
                    // Lockout duration has passed; clear the lockout flag
                    HttpContext.Session.SetInt32("Key", 0);
                    HttpContext.Session.Remove("LockoutEndTime");
                }
            }


            if (HttpContext.Session.GetInt32("Key") == null || HttpContext.Session.GetInt32("Key") < 3)
            // Check if the user is locked out
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    //get the role of the
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains("Registerer"))
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", $"Only registerers are allowed to log in here. Student and Teacher logins are seperate and can be seen from the navigation bar.");
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
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Account locked out. Please try again later.");
                        return View(model);
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Two-factor authentication is required for this account.");
                        return View(model);
                    }
                    else if (result.IsNotAllowed)
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Login is not allowed for this account.");
                        return View(model);
                    }
                    else
                    {
                        checkandsetSession();
                        int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                        model.logintrial = 4 - session_intermediate;
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }

                }
                else
                {
                    checkandsetSession();
                    int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                    model.logintrial = 4 - session_intermediate;
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
            }

            var lockoutEndTime = DateTime.UtcNow.AddMinutes(2);
            var lockoutEndTimeString = lockoutEndTime.ToString("HH:mm:ss");
            HttpContext.Session.SetString("LockoutEndTime", lockoutEndTimeString);
            ModelState.AddModelError("", "You are locked out for 2 minutes beacuse of several unsuccessfull attemps!!");
            return View(model);

        }

        [AllowAnonymous]
        
        public IActionResult GeneralLogin() 
        {
            // Retrieve the error message from TempData
            if (!User.Identity.IsAuthenticated)
            {
                TempData["errorMessage"] = "Your session has expired. Please log in again.";
            }
            var model = new LoginViewModel { logintrial = 5 };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> GeneralLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            var lockoutEndTimeString_2 = HttpContext.Session.GetString("LockoutEndTime");
            if (!string.IsNullOrEmpty(lockoutEndTimeString_2))
            {
                var lockoutEndTime_2 = DateTime.ParseExact(lockoutEndTimeString_2, "HH:mm:ss", CultureInfo.InvariantCulture);
                if (DateTime.UtcNow < lockoutEndTime_2)
                {
                    // User is still locked out
                    ModelState.AddModelError("", $"You are locked out. Please try again later. Time remaining: {lockoutEndTime_2 - DateTime.UtcNow:mm\\:ss} minutes ");
                    return View(model);
                }
                else
                {
                    // Lockout duration has passed; clear the lockout flag
                    HttpContext.Session.SetInt32("Key", 0);
                    HttpContext.Session.Remove("LockoutEndTime");
                }
            }
            if (HttpContext.Session.GetInt32("Key") == null || HttpContext.Session.GetInt32("Key") < 3)
            // Check if the user is locked out
            {

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    IdentityUser? user = await _userManager.FindByNameAsync(model.UserName);

                    if (user == null) { ModelState.AddModelError("", "Invalid username or password."); return View(model); }

                    var roles = await _userManager.GetRolesAsync(user);


                    if (roles.Contains("Registerer"))

                    {

                        TempData["success"] = "You are successfully logged in with a Registerer role.";
                        return RedirectToAction("Index", "Home");
                    }
                    else if (roles.Contains("Student"))

                    {

                        TempData["success"] = "You are successfully logged in with a Student role.";
                        return RedirectToAction("Index", "Home");
                    }
                    else if (roles.Contains("Teacher"))

                    {

                        TempData["success"] = "You are successfully logged in with a Teacher role.";
                        return RedirectToAction("Index", "Home");
                    }


                }
                else if (result.IsLockedOut)
                {
                    checkandsetSession();
                    int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                    model.logintrial = 4 - session_intermediate;
                    ModelState.AddModelError("", "Account locked out. Please try again later.");
                    return View(model);
                }
                else if (result.RequiresTwoFactor)
                {
                    checkandsetSession();
                    int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                    model.logintrial = 4 - session_intermediate;
                    ModelState.AddModelError("", "Two-factor authentication is required for this account.");
                    return View(model);
                }
                else if (result.IsNotAllowed)
                {
                    checkandsetSession();
                    int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                    model.logintrial = 4 - session_intermediate;
                    ModelState.AddModelError("", "Login is not allowed for this account.");
                    return View(model);
                }
                else
                { checkandsetSession();
                    int session_intermediate = (int)HttpContext.Session.GetInt32("Key");
                    model.logintrial = 4 - session_intermediate;
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }
            var lockoutEndTime = DateTime.UtcNow.AddMinutes(2);
            var lockoutEndTimeString = lockoutEndTime.ToString("HH:mm:ss");
            HttpContext.Session.SetString("LockoutEndTime", lockoutEndTimeString);
            ModelState.AddModelError("", "You are locked out for 2 minutes beacuse of several unsuccessfull attemps!!");
            return View(model);

        }


        [AllowAnonymous]
        public async Task<IActionResult> AccessDenied()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<IdentityRole?> allroles = _roleManager.Roles.ToList();
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles[0];
                if(role == null) { return NotFound(); }
                _logger.LogWarning("Access denied for user: {User}", User.Identity.Name); // not sur eif its neccessary
                TempData["errorMessage"] = $"You do not have the access to this action. Your Role is {role}";
                return View(allroles);

            }

            return RedirectToAction("GeneralLogin");
        }


    }
}
