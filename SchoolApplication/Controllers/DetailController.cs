using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SchoolApplication.Models;
using SchoolApplication.Data;

namespace SchoolApplication.Controllers
{
    public class DetailController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ObjectDbContext _objectDbContext;
        public DetailController(UserManager<IdentityUser> userManager, ObjectDbContext objectDbContext)
        {
            _userManager = userManager; 
            _objectDbContext = objectDbContext;
        }

        [Authorize(Roles="Student")]
        public async Task<IActionResult> StudentDetail(string name)
        {
            if (name == null) { return NotFound(); }

            Student? student =await _userManager.FindByNameAsync(name) as Student;
            if (student == null) { return NotFound(); }

            student.Grades["Math"] = student.Math;
            student.Grades["Science"] = student.Science;
            student.Grades["Language"] = student.Language;
            student.Grades["History"] = student.History;
            student.Grades["Sports"] = student.Sports;
            
            return View(student);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> TeacherDetail()
        {
            Teacher? teacher = await _userManager.GetUserAsync(User) as Teacher;
            if (teacher == null) { return NotFound(); }
            return View(teacher);
        }

        [Authorize(Roles = "Registerer")]
        public IActionResult SchoolDetail() 
        {
            return View();
        }

        [Authorize(Roles = "Teacher,Student")]
        public IActionResult ProductDetail(int? id)
        {
            if (id == null) { return NotFound(); }
            var product = _objectDbContext.Find<Product>(id);
            if(product == null) { return NotFound(); }
            return View(product);
        }
    }
}
