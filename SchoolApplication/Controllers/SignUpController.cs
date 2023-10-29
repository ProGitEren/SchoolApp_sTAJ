using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolApplication.Models;
using SchoolApplication.Models.ViewModels;

namespace SchoolApplication.Controllers


{
    [Authorize(Roles = "Registerer")]
    public class SignUpController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
   

        public SignUpController(UserManager<IdentityUser> userManager)
        {
                _userManager = userManager;
       
        }
        //public IActionResult StudentSignUp()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> StudentSignUp(RegistererSignUpViewModel model) 
        //{
        //    if (ModelState.IsValid) 
        //    {
        //        var user = new IdentityUser { 
        //            UserName = model.Email,
        //            Email = model.Email };
        //        var result = await _userManager.CreateAsync(user,model.Password);
        //        if (result.Succeeded) 
        //        {
                    
        //            return RedirectToAction("Index", "Home"); // Replace with appropriate action and controller
        //        }

        //        foreach (var error in result.Errors) 
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }

        //    }

        //    return View(model);


            
        //}


        //Get action
        public IActionResult RegistererSignUp() 
        {
            return View();
        }

       
        //Post action

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistererSignUp(RegistererSignUpViewModel model)
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
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.UserName,
                    Phone = model.Phone,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Registerer");
                    TempData["success"] = "Registerer successfully created and assigned to Registerer Role";
                    return RedirectToAction("Index", "Home");
                }
               
                //{
                //    await _signInManager.SignInAsync(user, isPersistent: false);
                //    return RedirectToAction("Index", "Home"); // Replace with appropriate action and controller
                //}

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("CustomError", error.Description);
                }
            }

            return View(model);
        }
    }
}

