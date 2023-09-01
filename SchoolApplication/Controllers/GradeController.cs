using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SchoolApplication.Models;
using SchoolApplication.Models.ViewModels;

namespace SchoolApplication.Controllers
{
    [Authorize(Roles ="Teacher")]
    public class GradeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        

        public GradeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            
        }

        public async Task<IActionResult> Index()
        {
            Teacher? teacher = await _userManager.GetUserAsync(User) as Teacher;
            IList<IdentityUser> users = await _userManager.GetUsersInRoleAsync("Student");
            List<Student?> studentlist = users.Select(u=>u as Student).ToList();
            var model = new GradeIndexViewModel
            {
                lecturetype = teacher.lecture.ToString(),
                students = studentlist,
            };
            return View(model);
        }

        public async Task<IActionResult> Edit(string id) 
        {
            if (id == null) { return NotFound(); }

            Student? student = await _userManager.FindByIdAsync(id) as Student;
            Teacher? teacher = await _userManager.GetUserAsync(User) as Teacher;

            if (student == null || teacher == null) { return NotFound(); }

            string lecture = teacher.lecture.ToString();
            
            var model = new GradeEditViewModel
            {
                Id = student.Id,
                Grade = student.GetGrades()[lecture],
                Name = student.Name,
                Department=student.Department 
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(GradeEditViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                var student = await _userManager.FindByIdAsync(model.Id) as Student;
                var teacher = await _userManager.GetUserAsync(User) as Teacher;

                if (student == null || teacher == null) { return NotFound(); }

                string lecture = teacher.lecture.ToString();
                student.GetGrades()[lecture] = model.Grade;
               
                var result = await _userManager.UpdateAsync(student);

                if (result.Succeeded)
                {
                    TempData["success"] = "Grade successfully updated.";
                    return RedirectToAction("Index", "Grade");
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
