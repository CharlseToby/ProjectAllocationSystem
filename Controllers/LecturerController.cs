using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

            var assignedStudentsId = _dbContext.LecturerStudentNodes
                .Where(x => x.LecturerId == lecturer.Id)
                .Select(x => x.StudentId).ToList();
            var assignedStudents = _dbContext.Users
                .Where(x => x.Role == Role.Student)
                .Where(x => assignedStudentsId.Contains(x.Id))
                .ToList();

            var assignedStudentsModel = assignedStudents.Select(y => new AssignedStudent
            {
                Student = y,
                PreferenceMatch = y.ProjectPreferences.Intersect(lecturer.ProjectPreferences)
                    .Select(x => x.Preference)
                    .ToList()
            }).ToList();

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
                AssignedStudents = assignedStudentsModel,
                AllPreferences = await _dbContext.ProjectPreferences.ToListAsync(),
                SelectedPreferences = lecturer.ProjectPreferences
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> StudentProfile(string id)
        {
            var student = await _userManager.FindByIdAsync(id);
            bool assignedToLecturer = _dbContext.LecturerStudentNodes.FirstOrDefault(x => x.StudentId == id) != null;
            var vm = new StudentProfileVM
            {
                ApplicationUser = student,
                AssignedToLecturer = assignedToLecturer
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyPreferences(string[] selectedPreferences)
        {
            var lecturer = await _userManager.GetUserAsync(User);
            var prefsToAdd = selectedPreferences.Where(x => !lecturer.ProjectPreferences.Select(x => x.Preference).Contains(x));
            var prefsToRemove = lecturer.ProjectPreferences.Select(x => x.Preference).Where(x => !selectedPreferences.Contains(x));

            var projectPrefsToAdd = await _dbContext.ProjectPreferences.Where(x => prefsToAdd.Contains(x.Preference))
                .ToListAsync();
            var projectPrefsToRemove = await _dbContext.ProjectPreferences.Where(x => prefsToRemove.Contains(x.Preference))
                .ToListAsync();


            lecturer.ProjectPreferences.AddRange(projectPrefsToAdd);
            lecturer.ProjectPreferences.RemoveAll(x => projectPrefsToRemove.Contains(x));
            await _userManager.UpdateAsync(lecturer);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChatStudent(string studentId)
        {
            //TODO: Add DB contraint to prevent duplicate students in LecturerStudentNode table
            var node = await _dbContext.LecturerStudentNodes.FirstOrDefaultAsync(x => x.StudentId == studentId);
            return RedirectToAction("Index", "Chat", new { nodeId = node.Id });
        }
    }
}
