using Microsoft.AspNetCore.Identity;
using SchoolApplication.Models;

namespace SchoolApplication.Expire
{
    public class StudentService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public StudentService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task CheckAndDeleteExpiredStudents()
        {
            DateTime currentTime = DateTime.Now;
            
            // Find and delete expired students
            var expiredStudents = _userManager.Users
                .Where(user => user is Student && DateTime.UtcNow > ((Student)user).expiredate)
                .ToList();

            foreach (var student in expiredStudents)
            {
                await _userManager.DeleteAsync(student);
                // Optionally, you can log or perform other actions here
            }
        }
    }
}
