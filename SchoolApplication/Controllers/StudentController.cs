using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolApplication.Data;
using SchoolApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SchoolApplication.Models.ViewModels;
using NuGet.Protocol;
using System.Diagnostics;

namespace SchoolApplication.Controllers
{
    [Authorize(Roles = "Registerer")]
    public class StudentController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public StudentController(UserManager<IdentityUser> userManager) 
        {
            _userManager = userManager;      
        }
        // GET: StudentController

        //[Authorize(Roles = "Registerer")]
        public async Task<IActionResult> Index()
        {
            IList<IdentityUser> identitystudents = await _userManager.GetUsersInRoleAsync("Student");
            List<Student?> students = identitystudents.Select(u => u as Student).ToList();
            foreach(Student? student in students) 
            {
                if(student == null) continue;
                student.Grades["Math"] = student.Math;
                student.Grades["Science"] = student.Science;
                student.Grades["Language"] = student.Language;
                student.Grades["History"] = student.History;
                student.Grades["Sports"] = student.Sports;

            }
            return View(students);
        }

        // GET: StudentController/Details/5

        //        public IActionResult Details(int id)
        //        {
        //            return View();
        //        }

        // GET: StudentController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles ="Registerer")]
        public async Task<IActionResult> Create(StudentSignUpViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View(model);
                }


                var user = new Student
                {
                    Department = model.Department,
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.UserName,
                    Phone = model.Phone,
                    Discount = model.Discount,
                    expiredate = model.expiretime
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {

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
                        registerer.AssignPriceToStudent(user);
                    }
                    catch(Exception e) 
                    {
                        ModelState.AddModelError("DepartmentError", e.Message);
                            return View(model);
                    }
                }
                    TempData["success"] = "Student successfully created and price is successfully assigned to the student.";
                    await _userManager.AddToRoleAsync(user, "Student");
                    return RedirectToAction("Index", "Student");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("CustomError", error.Description);
                }
            }
            return View(model);
        }

        // GET: StudentController/Edit/5

        //[Authorize(Roles ="Registerer")]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student? student = _userManager.FindByIdAsync(id).Result as Student;

            var model = new StudentUpdateViewModel
            {
                Name = student.Name,
                Department = student.Department,
                Discount = student.Discount,
                Phone = student.Phone,
                Id = student.Id
            };

            
            if (student == null || model == null) { return NotFound(); }

            return View(model);
        }

        //POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles ="Registerer")]
        public async Task<IActionResult> Edit(StudentUpdateViewModel model)
        {
            
            if (model == null) { return NotFound(); }
            
            if (ModelState.IsValid) 
            {
                Student? student = _userManager.FindByIdAsync(model.Id).Result as Student;
               
                if (student == null) { return NotFound(); }
                //Changing the properties of student
                student.Department = model.Department;
                student.Discount = model.Discount;
                student.Name = model.Name;
                student.Phone = model.Phone;


                var result = await _userManager.UpdateAsync(student);
                if (result.Succeeded) 
                {
                    TempData["success"] = "Student successfully updated.";
                    return RedirectToAction("Index","Student");
                }
                foreach(var error in result.Errors) { 
                    ModelState.AddModelError("", error.Description); 
                }

            }
            
            return View(model);   
        }

        // GET: StudentController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }   

            Student? student = await _userManager.FindByIdAsync(id) as Student;
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
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

            IdentityUser? student = await _userManager.FindByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(student);
            if (result.Succeeded)
            {
                TempData["success"] = "Student successfully deleted.";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(student);
        }
    }
}
