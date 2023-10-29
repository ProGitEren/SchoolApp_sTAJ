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
            foreach(Student student in studentlist) 
            {
                
                if (student == null) continue;
                student.Grades["Math"] = student.Math;
                student.Grades["Science"] = student.Science;
                student.Grades["Language"] = student.Language;
                student.Grades["History"] = student.History;
                student.Grades["Sports"] = student.Sports;
            }


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

            //string lecture = teacher.lecture.ToString();

            switch (teacher.lecture)
            {
                case lectureType.Math:
                    student.Grades["Math"] = student.Math;
                    break;
                case lectureType.Science:
                    student.Grades["Science"] = student.Science;
                    break;
                case lectureType.Language:
                    student.Grades["Language"] = student.Language;
                    break;
                case lectureType.History:
                    student.Grades["History"] = student.History;
                    break;
                case lectureType.Sports:
                    student.Grades["Sports"] = student.Sports;
                    break;
                default:
                    throw new Exception("Invalid Lecture Type ");
            }

            //        student.Grades["Math"] = student.Math;
            //student.Grades["Science"] = student.Science;
            //student.Grades["Language"] = student.Language;
            //student.Grades["History"] = student.History;
            //student.Grades["Sports"] = student.Sports;

            var model = new GradeEditViewModel
            {
                Id = student.Id,
                Grade = student.Grades[teacher.lecture.ToString()],
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

                switch (teacher.lecture) 
                {
                    case lectureType.Math:
                        student.Math = model.Grade;
                        break;
                    case lectureType.Science:
                        student.Science = model.Grade;
                        break;
                    case lectureType.Language:
                        student.Language = model.Grade;
                        break;
                        case lectureType.History:
                        student.History = model.Grade;
                        break;
                        case lectureType.Sports:
                        student.Sports = model.Grade;
                        break;
                    default:
                        throw new Exception("Invalid Lecture Type!");

                }
               
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
