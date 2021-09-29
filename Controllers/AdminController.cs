using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAllocationSystem.Data;
using ProjectAllocationSystem.DTOs;
using ProjectAllocationSystem.Models;
using ProjectAllocationSystem.Services;
using ProjectAllocationSystem.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Controllers
{
    // TODO: Model Validation and Error Handling for controller actions
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly ApplicationDbContext _dbContext;

        public AdminController(AdminService adminService, ApplicationDbContext dbContext)
        {
            _adminService = adminService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var preferences = await _dbContext.ProjectPreferences.Select(x => x.Preference).ToListAsync();
            var vm = new IndexVM
            {
                Preferences = preferences
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string firstName, string lastName, string schoolId, string role)
        {            
            var user = await _adminService.CreateUser(firstName, lastName, schoolId, role);            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPreference(string preference)
        {
            await _adminService.AddPreference(preference);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePreference(string preference)
        {
            await _adminService.RemovePreference(preference);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> BulkAddUsers([FromForm] IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                List<ApplicationUserDTO> applicationUsers;
                var reader = new StreamReader(stream);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    TrimOptions = TrimOptions.Trim,
                    HeaderValidated = (x) => { },
                    MissingFieldFound = null
                };

                using (var csv = new CsvReader(reader, config))
                {
                    applicationUsers = csv.GetRecords<ApplicationUserDTO>().ToList();
                }
                stream.Close();                

                await _adminService.BulkCreateUsers(applicationUsers);
                return RedirectToAction("Index");
            }
        }
    }
}
