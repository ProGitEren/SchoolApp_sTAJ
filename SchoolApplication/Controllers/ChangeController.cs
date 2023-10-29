using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SchoolApplication.Models.ViewModels;
using SchoolApplication.Messages;

namespace SchoolApplication.Controllers
{
    public class ChangeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ChangeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;    
            _signInManager = signInManager;
        }
        public IActionResult ChangePassword(string role)
        {
            EmailVerifyViewModel model = new EmailVerifyViewModel { userrole = role };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(EmailVerifyViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await _userManager.FindByEmailAsync(model.email);
                if (user == null) 
                {
                    ModelState.AddModelError("NotVerifiedEmail", "This email is not found in our system. Please try again.");
                    return View(model);
                }
                var roles = await _userManager.GetRolesAsync(user);
                switch (model.userrole) 
                {
                    
                    case "Registerer":
                        
                        if (roles.Contains("Registerer")) 
                        {
                            return RedirectToAction("EmailVerified", new { id = user.Id,email=model.email });

                        }
                        else 
                        {
                            ModelState.AddModelError("", "This email is not belong to your role. Please give your email!!!!");
                            return View(model);
                        }
                        
                    case "Student":

                        if (roles.Contains("Student"))
                        {
                            return RedirectToAction("EmailVerified", new { verifiedmodel = new EmailVerifyViewModel { email = model.email, userrole = model.userrole } });

                        }
                        else
                        {
                            ModelState.AddModelError("", "This email is not belong to your role. Please give your email!!!!");
                            return View(model);
                        }
                        
                    case "Teacher":

                        if (roles.Contains("Teacher"))
                        {
                            return RedirectToAction("EmailVerified", new { verifiedmodel = new EmailVerifyViewModel { email = model.email, userrole = model.userrole } });

                        }
                        else
                        {
                            ModelState.AddModelError("", "This email is not belong to your role. Please give your email!!!!");
                            return View(model);
                        }
                        
                       
                    default:
                        throw new Exception("Not Valid Role Name.");
                        

                }
                
            }

            // If the model is not valid, return to the form with validation errors
            return View(model);
        }

        public async Task<IActionResult> EmailVerified(string id, string email) 
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(email);
            var model = new ChangePasswordViewModel {Id=user.Id};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EmailVerified(ChangePasswordViewModel model) 
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user based on the userId
                IdentityUser? user = await _userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.oldPassword, false);
                    // Update the user's password in your database

                    if (signInResult.Succeeded)
                    {
                        var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);

                        if (result.Succeeded)
                        {
                            // Password changed successfully, redirect to a success page
                            TempData["success"] = "Password successfully Changed";
                            return RedirectToAction("Index","Home");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    else
                    {
                        // User authentication failed with the old password
                        ModelState.AddModelError("OldPassword", "Invalid old password. Please try again.");
                    }
                }
                else
                {
                    // Handle the case where the user is not found
                    ModelState.AddModelError("", "User not found.");
                }
            }

            return View(model);
        
        
        
        }

    }
}
