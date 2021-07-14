using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAllocationSystem.Data;
using ProjectAllocationSystem.Models;
using ProjectAllocationSystem.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext applicationDbContext, 
            UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = await GetIndexViewModel(user);

            return View(vm);
        }

        private async Task<IndexVM> GetIndexViewModel(ApplicationUser user)
        {
            string supervisorId = _applicationDbContext.LecturerStudentNodes.FirstOrDefault(x => x.StudentId == user.Id)?
                .LecturerId;

            var vm = new IndexVM
            {
                FirstName = user.FirstName,
                StudentPreferences = user.ProjectPreferences,
                AllPreferences = await _applicationDbContext.ProjectPreferences.ToListAsync()
            };

            if (supervisorId is not null)
            {
                var supervisor = await _applicationDbContext.Users.FindAsync(supervisorId);
                vm.SupervisorId = supervisorId;
                vm.SupervisorFirstName = supervisor.FirstName;
                vm.SupervisorLastName = supervisor.LastName;
            }

            return vm;
        }

        public async Task<IActionResult> ChoosePreferences(List<string> preferences)
        {
            var user = await _userManager.GetUserAsync(User);

            var preferencesToAdd = await _applicationDbContext.ProjectPreferences.Where(x => preferences.Contains(x.Preference))
                .ToListAsync();
            user.ProjectPreferences.AddRange(preferencesToAdd);
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }
    }
}
