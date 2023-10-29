using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SchoolApplication.Data;
using SchoolApplication.Messages;

namespace SchoolApplication.Controllers
{

    
    public class MessageController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ObjectDbContext _objectDbContext;

        public MessageController(UserManager<IdentityUser> userManager, ObjectDbContext objectDbContext)
        {
            _userManager = userManager;
            _objectDbContext = objectDbContext;
        }

        //List of Messages 

        [Authorize(Roles = "Registerer")]
        public async Task<IActionResult> Index()
        {
            IdentityUser? registerer = await _userManager.GetUserAsync(User);
            if (registerer == null) {return NotFound();}
            try
            {
                List<MessageContainer> relevantmessages = _objectDbContext.MessageContainer
                    .Where(x => x.receiver == registerer.Email)
                    .OrderByDescending(x => x.createdtime)
                    .ToList();
                return View(relevantmessages);
            }
            catch (Exception ex) 
            {
                TempData["error"] = ex.Message.ToString();
                return RedirectToAction("Index", "Home");
                
            }

            
        }
        [Authorize(Roles="Student,Teacher")]
        public async Task<IActionResult> RequestIndex() 
        {
            IdentityUser? user = await _userManager.GetUserAsync(User);

            if (user == null) { return NotFound();}

            List<MessageContainer> messagebox = _objectDbContext.MessageContainer
                .Where(x=>x.messager == user.Email)
                .OrderByDescending(x=>x.createdtime)
                .ToList();
            return View(messagebox);
        }

        [Authorize]

        public IActionResult Read(int? id) 
        {
            if (id == null) { return NotFound(); }

            MessageContainer? message = _objectDbContext.Find<MessageContainer>(id);

            if (message == null) { return NotFound(); } 
            
            return View(message);
        }

        [Authorize]

        public async Task<IActionResult> Delete(int? id) {
        
            if (id == null) { return NotFound(); }

            MessageContainer? message = _objectDbContext.Find<MessageContainer>(id);
            if(message == null) { return NotFound(); }
            try
            {
                _objectDbContext.Remove<MessageContainer>(message);
                await _objectDbContext.SaveChangesAsync();
                TempData["success"] = "Message Successfully Deleted";
                if (User.IsInRole("Registerer"))
                {
                    return RedirectToAction("Index", "Message");
                }
                else
                {
                    return RedirectToAction("RequestIndex", "Message");
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        [Authorize(Roles ="Registerer")]

        public async Task<IActionResult> Accept(int? id) 
        {
            if (id == null) { return NotFound(); }
            MessageContainer? messagebox = _objectDbContext.Find<MessageContainer>(id);
            if (messagebox == null) { return NotFound(); }
            messagebox.accepted = true;
            try
            {
                await _objectDbContext.SaveChangesAsync();
                TempData["success"] = "Message Accepted successfully.";
                return RedirectToAction("Index", "Message");
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message.ToString());
            }
            
        }
    }
}
