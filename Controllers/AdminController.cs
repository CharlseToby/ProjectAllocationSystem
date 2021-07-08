using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAllocationSystem.Data;
using ProjectAllocationSystem.Services;
using ProjectAllocationSystem.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly ApplicationDbContext _dbContext;

        public AdminController(AdminService adminService, ApplicationDbContext dbContext)
        {
            _adminService = adminService;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var preferences = await _dbContext.ProjectPreferences.Select(x => x.Preference).ToListAsync();
            var vm = new IndexVM
            {
                Preferences = preferences
            };

            return View(vm);
        }
    }
}
