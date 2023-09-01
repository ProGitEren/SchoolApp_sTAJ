using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SchoolApplication.Models;
using SchoolApplication.Models.ViewModels;
using NuGet.DependencyResolver;

namespace SchoolApplication.Controllers
{
    [Authorize(Roles ="Registerer")]
    public class TeacherController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        

        public TeacherController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;   
        }
        public async Task<IActionResult> Index()
        {
            IList<IdentityUser> identity = await _userManager.GetUsersInRoleAsync("Teacher");
            List<Teacher?> teachers = identity.Select(u=>u as Teacher).ToList();
            return View(teachers);
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(TeacherSignUpViewModel model) 
        {
            if (ModelState.IsValid) 
            {

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View(model);
                }

                var teacher = new Teacher
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.UserName,
                    Phone = model.Phone,
                };
                switch(model.Lecture) 
                {
                    case 0:
                        teacher.lecture = lectureType.Math;
                        break;
                    case 1:
                        teacher.lecture = lectureType.Science;
                        break;
                    case 2:
                        teacher.lecture = lectureType.Language;
                        break;
                    case 3:
                        teacher.lecture = lectureType.History;
                        break;
                    case 4:
                        teacher.lecture = lectureType.Sports;
                        break;
                    default:
                        throw new Exception("Invalid Lecture Type!!");
                        
                }
                switch (model.Level) 
                {
                    case 0:
                        teacher.Type = teacherType.Advanced;
                        break;
                    case 1:
                        teacher.Type = teacherType.Professor;
                        break;
                    case 2:
                        teacher.Type = teacherType.HeadofDepartment;
                        break;
                    case 3:
                        teacher.Type = teacherType.Deputy;
                        break;
                    default:
                        throw new Exception("Invalid teacher level!!");
                        
                }
                var result = await _userManager.CreateAsync(teacher, model.Password);
                if (result.Succeeded)
                {

                    var currentUser = await _userManager.GetUserAsync(User);
                    Registerer? registerer = currentUser as Registerer;

                    //// Same procedure second version
                    //var regist2 = await _userManager.FindByNameAsync(User.Identity.Name);
                    //Registerer? regist3 = regist2 as Registerer;

                    ////Same procedure third version
                    //if (currentUser != null && currentUser is Registerer registerer) { registerer.AssignPriceToStudent(user); }
                    if (registerer != null)
                    {
                        try
                        {
                            registerer.AssignSalaryToTeacher(teacher);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("LevelError", e.Message);
                            return View(model);
                        }
                    }
                    TempData["success"] = "Teacher created successfully and salary is successfully assigned.";
                    await _userManager.AddToRoleAsync(teacher, "Teacher");
                    return RedirectToAction("Index", "Teacher");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("CustomError", error.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)

        {
            if (id == null) { return NotFound(); }
            Teacher? teacher = await _userManager.FindByIdAsync(id) as Teacher;
            if(teacher == null) { return NotFound(); }
            
            var model = new TeacherUpdateViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Phone = teacher.Phone,
                Lecture =(int) teacher.lecture,
                Level = (int) teacher.Type
            };
            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(TeacherUpdateViewModel model) 
        {
            if (ModelState.IsValid)
            {
                Teacher? teacher = await _userManager.FindByIdAsync(model.Id) as Teacher;

                if (teacher == null) { return NotFound(); }
                
                teacher.Name = model.Name;
                teacher.Phone = model.Phone;

                switch (model.Lecture)
                {
                    case 0:
                        teacher.lecture = lectureType.Math;
                        break;
                    case 1:
                        teacher.lecture = lectureType.Science;
                        break;
                    case 2:
                        teacher.lecture = lectureType.Language;
                        break;
                    case 3:
                        teacher.lecture = lectureType.History;
                        break;
                    case 4:
                        teacher.lecture = lectureType.Sports;
                        break;
                    default:
                        throw new Exception("Invalid Lecture Type!!");

                }
                switch (model.Level)
                {
                    case 0:
                        teacher.Type = teacherType.Advanced;
                        break;
                    case 1:
                        teacher.Type = teacherType.Professor;
                        break;
                    case 2:
                        teacher.Type = teacherType.HeadofDepartment;
                        break;
                    case 3:
                        teacher.Type = teacherType.Deputy;
                        break;
                    default:
                        throw new Exception("Invalid teacher level!!");

                }

                var result = await _userManager.UpdateAsync(teacher);
                if (result.Succeeded)
                {
                    TempData["success"] = "Teacher successfully updated.";
                    return RedirectToAction("Index", "Teacher");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id) 
        {
            if (id == null) { return NotFound(); }

            Teacher? teacher = await _userManager.FindByIdAsync(id) as Teacher;

            if (teacher == null) { return NotFound(); }

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeletePost(string id) 
        {
            if (id == null) { return NotFound(); }

            IdentityUser? user = await _userManager.FindByIdAsync(id);

            if (user == null) { return NotFound(); }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded) 
            {
                TempData["success"] = "Teacher successfully deleted.";
                return RedirectToAction("Index", "Teacher");
            
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(user);
        }

    }



}
