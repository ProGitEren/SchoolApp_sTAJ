using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SchoolApplication.Models;
using SchoolApplication.Models.ViewModels;
using NuGet.Protocol.Plugins;
using SchoolApplication.Messages;
using SchoolApplication.Data;

namespace SchoolApplication.Controllers
{

    [Authorize]
    //This controller will be for registerers only
    public class ContactController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ObjectDbContext _objectDbContext;

        public ContactController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,ObjectDbContext objectDbContext)
        {
                _userManager = userManager;
                _signInManager = signInManager;
                _objectDbContext = objectDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var userlist = await _userManager.GetUsersInRoleAsync("Registerer");
            List<Registerer?> registererlist = userlist.Select(x => x as Registerer).ToList();
            return View(registererlist);
        }

        [Authorize(Roles = "Registerer")]
        public async Task<IActionResult> Edit(string id) 
        {
            if (id == null) { return NotFound(); }
            Registerer? registerer = await _userManager.FindByIdAsync(id) as Registerer;
            if (registerer == null) { return NotFound(); }
            RegistererUpdateViewModel model = new RegistererUpdateViewModel 
            {
                Id=id,
                email = registerer.Email,
                Phone = registerer.Phone,
                Name = registerer.Name,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Registerer")]
        public async Task<IActionResult> Edit(RegistererUpdateViewModel model)
        {

            if (model == null) { return NotFound(); }

            if (ModelState.IsValid)
            {
                Registerer? registerer = _userManager.FindByIdAsync(model.Id).Result as Registerer;

                if (registerer == null) { return NotFound(); }

                var oldusername = registerer.UserName;

                //Changing the properties of student
                registerer.Name = model.Name;
                registerer.Email = model.email;
                registerer.Phone = model.Phone;
                registerer.UserName = model.email;


                var result = await _userManager.UpdateAsync(registerer);
                if (result.Succeeded)
                {
                    if (oldusername != model.email)
                    {
                        await _signInManager.SignOutAsync(); // Sign out the user
                        await _signInManager.SignInAsync(registerer, isPersistent: false); // Sign in with the updated user
                    }
                    TempData["success"] = "Registerer successfully updated.";
                    return RedirectToAction("Index", "Contact");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(model);
        }

        // GET: StudentController/Delete/5
        [Authorize(Roles ="Registerer")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Registerer? registerer = await _userManager.FindByIdAsync(id) as Registerer;
            if (registerer == null)
            {
                return NotFound();
            }

            return View(registerer);
        }

        //POST: StudentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser? registerer = await _userManager.FindByIdAsync(id);

            if (registerer == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(registerer);
            if (result.Succeeded)
            {
                TempData["success"] = "Registerer successfully deleted.";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registerer);
        }

        [Authorize]
        public async Task<IActionResult> Request(string id) 
        {

            if (id == null) { return NotFound(); }

            Registerer? registerer = await _userManager.FindByIdAsync(id) as Registerer;
            if (registerer == null) { return NotFound(); }

            var model = new RequestViewModel { Id =  registerer.Id, Name = registerer.Name};

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Request(RequestViewModel model) 
        {
            if (ModelState.IsValid)
            { 

            //new folder will be added for message containirization
            IdentityUser? Messager = await _userManager.GetUserAsync(User);
            if (Messager == null) { return NotFound(); }    

            IdentityUser? Reciever = await _userManager.FindByIdAsync(model.Id);
            if (Reciever  == null) { return NotFound(); }
            

            var Message = model.Message;

                if (Message == null) { return NotFound(); }

                MessageContainer messageContainer = new MessageContainer
                {
                    message= Message,
                    receiver= Reciever.Email ,
                    messager = Messager.Email ,
                    subject = model.Subject
                };

                try
                {
                    //This will add the message to the message container which will be then filtered to show to the registerers in their layout as Requests
                    _objectDbContext.Add(messageContainer);
                    _objectDbContext.SaveChanges();
                    
                    TempData["success"] = "Message successfully stored.";
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return View(model);
        }

    }
}
