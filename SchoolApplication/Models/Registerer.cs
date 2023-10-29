using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SchoolApplication.Data.Money;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.CodeAnalysis.Options;


namespace SchoolApplication.Models
{
    public class Registerer : ApplicationUser
    {
        public void AssignSalaryToTeacher(Teacher teacher) 
        {
            decimal intermediate = 0;

            if (teacher.Type == teacherType.Advanced)
            {
                teacher.SetSalary(TeacherSalary.advanced);
                intermediate = TeacherSalary.advanced;
            }
            else if (teacher.Type == teacherType.Professor)
            {
                teacher.SetSalary(TeacherSalary.professor);
                intermediate = TeacherSalary.professor;
            }
            else if (teacher.Type == teacherType.HeadofDepartment) 
            {
                teacher.SetSalary(TeacherSalary.headofdepartment);
                intermediate = TeacherSalary.headofdepartment;
            }
            else 
            {
                teacher.SetSalary(TeacherSalary.deputy);
                intermediate = TeacherSalary.deputy;
            }
            //salary[teacher] = intermediate; problem in this part maybe 
            teacher.SetSalary(intermediate);

        }

        public void AssignPriceToStudent(Student  student) 
        {
            decimal departmentPrice = 0;

            switch (student.Department)
            {
                case "0":
                departmentPrice = DepartmentPrices.machineeng;
                break;
            case "1":
                departmentPrice = DepartmentPrices.eleceng;
                break;
            case "3":
                departmentPrice = DepartmentPrices.chemeng;
                break;
            case "4":
                departmentPrice = DepartmentPrices.indeng;
                break;
            case "2":
                departmentPrice = DepartmentPrices.compeeng;
                break;
            case "5":
                departmentPrice = DepartmentPrices.medicine;
                break;
            case "6":
                departmentPrice = DepartmentPrices.psychology;
                break;
            case "7":
                departmentPrice = DepartmentPrices.mediaanddesign;
                break;
            case "9":
                departmentPrice = DepartmentPrices.business;
                break;
            case "8":
                departmentPrice = DepartmentPrices.economy;
                    break;
            default:
                    throw new ArgumentException("Invalid Department.");

            }
            decimal finalPrice = departmentPrice * ((100m - student.Discount) / 100m);
            student.SetPrice(finalPrice);
            

        }
        public void SetDiscount(Student student,int discount) 
        {
            student.Discount = discount;
            discounts[student] = discount;

        }
       
        [Required]
        public Dictionary<Student,int> discounts;
        [Required]
        public Dictionary<Teacher, decimal> salary;

        public decimal Balance { get; set; } = 0;
    }
}
