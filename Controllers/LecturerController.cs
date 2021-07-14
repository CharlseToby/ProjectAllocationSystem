using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAllocationSystem.Data;
using ProjectAllocationSystem.Models;
using ProjectAllocationSystem.ViewModels.Lecturer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public LecturerController(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lecturer = await _userManager.GetUserAsync(User);            

            var assignedStudentsId = _dbContext.LecturerStudentNodes.Select(x => x.StudentId).ToList();
            var assignedStudents = await _dbContext.Users.Where(x => assignedStudentsId.Contains(x.Id))
                .Select(y => new AssignedStudent
                {
                    Student = y,
                    PreferenceMatch = y.ProjectPreferences.Intersect(lecturer.ProjectPreferences)
                    .Select(x => x.Preference)
                    .ToList()
                })
                .ToListAsync();
            /*var unassignedStudents = _dbContext.Users.Where(x => !assignedStudentsId.Contains(x.Id))
                .Where(x => x.ProjectPreferences
                .Intersect(lecturer.ProjectPreferences)
                .Any())
                .Select(x => new WaitingStudent 
                {
                    Student = x,
                    PreferenceMatch = x.ProjectPreferences.Intersect(lecturer.ProjectPreferences)
                    .Select(x => x.Preference)
                    .ToList()
                })
                .ToList();*/

            var vm = new IndexVM
            {
                AssignedStudents = assignedStudents
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> StudentProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            bool assignedToLecturer = _dbContext.LecturerStudentNodes.FirstOrDefault(x => x.StudentId == id) != null;
            var vm = new StudentProfileVM
            {
                ApplicationUser = user,
                AssignedToLecturer = assignedToLecturer
            };

            return View(vm);
        }        
    }
}
