using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolApplication.Controllers
{

    [Authorize(Roles ="Registerer")]
    public class RegistererController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
