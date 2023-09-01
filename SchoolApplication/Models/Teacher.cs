using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models
{
    public enum teacherType 
    {
        Advanced,
        Professor,
        HeadofDepartment,
        Deputy
    };
    public enum lectureType 
    
    {
        Math,
        Science,
        Language,
        History,
        Sports
    };
    public class Teacher : ApplicationUser
    {
     

    [Required]
        public teacherType Type { get; set; }
        [Required]
        public lectureType lecture { get; set; }

        public int SpentTimeInSchoolInDays 
        {
            get
            {
                int total = 0;
                total += (DateTime.Now.Year - CreatedTime.Year) * 365;
                total += (DateTime.Now.Month - CreatedTime.Month) * 30;
                total += DateTime.Now.Day - CreatedTime.Day;
                return total;
            } 
        }

        
        public void setGrade(Student student, decimal newgrade) 
        {
            if (student.GetGrades().ContainsKey(lecture.ToString()))
            {
                student.GetGrades()[lecture.ToString()] = newgrade;
            }
            else 
            {
                throw new Exception("Not valid lecture type");
            }
        }

        private decimal _privateSalary;

        public decimal ProtectedSalary 
        {
            get { return _privateSalary; }
           
        }

        internal void SetSalary(decimal newSalary) 
        {
            _privateSalary = newSalary;
        }
  
    }
}
