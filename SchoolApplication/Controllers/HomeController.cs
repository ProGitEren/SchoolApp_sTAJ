using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApplication.Data;
using SchoolApplication.Models;
using SchoolApplication.Models.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace SchoolApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ObjectDbContext _objectDbContext;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ObjectDbContext objectDbContext, ApplicationDbContext applicationDbContext, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _objectDbContext = objectDbContext;
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TokenTaking() 
        {
            var registererexist = _userManager.GetUsersInRoleAsync("Registerer").Result.ToList();
            if (registererexist.Count != 0) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Search(string searchQuery)
        {
            if(searchQuery == null) { return NotFound(); }
            var searchLower = searchQuery.ToLower();

            // Perform searches on different object types (Teacher, Student, Registerer, MessageContainer)
            var teachers = await _applicationDbContext.Teachers.ToListAsync();
            var students = await _applicationDbContext.Students.ToListAsync();
            var registerers = await _applicationDbContext.Registerers.ToListAsync();
            

            var teacherResults = teachers.Where(t => t.Name.Contains(searchLower) || t.lecture.ToString().Contains(searchLower) ||t.Type.ToString().Contains(searchLower)).ToList();
            var studentResults = students.Where(s => s.Name.Contains(searchLower) || s.Department.Contains(searchLower)).ToList();
            var registererResults = registerers.Where(r => r.Name.Contains(searchLower)).ToList();
            var messageContainerResults = _objectDbContext.MessageContainer.Where(m => m.message.Contains(searchLower) || m.receiver.Contains(searchLower) || m.messager.Contains(searchLower)).ToList();
            var productsResults = _objectDbContext.Products.Where(m=>m.Title.Contains(searchLower) || m.Author.Contains(searchLower)|| m.Lecture.Contains(searchLower)).ToList();

            var matchingRoleNames = _applicationDbContext.Roles
            .Where(r => r.Name.Contains(searchLower))
            .Select(r => r.Name)
            .ToList();

            // Retrieve users who belong to the matching roles
            var usersInMatchingRoles = new List<IdentityUser>();

            foreach (var roleName in matchingRoleNames)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                usersInMatchingRoles.AddRange(usersInRole);
            }

            //To prevent from adding same registerers teachers or students twice we remove the smae identities from the rolebasedresult to prevent this
            usersInMatchingRoles.RemoveAll(user =>
            teacherResults.Any(teacher => teacher.Id == user.Id) ||
            studentResults.Any(student => student.Id == user.Id) ||
            registererResults.Any(registerer => registerer.Id == user.Id));

            // Combine all search results into a single list
            var searchResults = new List<object>();
            searchResults.AddRange(teacherResults);
            searchResults.AddRange(studentResults);
            searchResults.AddRange(registererResults);
            searchResults.AddRange(messageContainerResults);
            searchResults.AddRange(usersInMatchingRoles);
            searchResults.AddRange(productsResults);





            // Populate the ViewModel with the results (you can also use ViewBag or TempData)
            var model = new SearchViewModel
            {
                SearchQuery = searchQuery,
                SearchResults = searchResults
            };

            // Return the ViewModel to the view
            return View(model);
        }

        [Authorize]

        public IActionResult Balance()
        {
            var user = _userManager.GetUserAsync(User).Result as ApplicationUser;
            //var roles= _userManager.GetRolesAsync(user).Result;
            //if (user is Teacher){ user = user as Teacher; }
            //if (user is Student)
            if (user == null) { return NotFound(); }
            return View(user);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}