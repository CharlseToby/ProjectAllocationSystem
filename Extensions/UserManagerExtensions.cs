using Microsoft.AspNetCore.Identity;
using ProjectAllocationSystem.Models;
using ProjectAllocationSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task SeedDatabase(this UserManager<ApplicationUser> userManager, AdminService adminService)
        {
            var admin = new ApplicationUser
            {
                FirstName = Constants.TestAdminLastName,
                LastName = Constants.TestAdminLastName,
                SchoolId = Constants.TestAdminUserName,
                UserName = Constants.TestAdminUserName,
                Role = Role.Admin,
            };

            var lecturer = new ApplicationUser
            {
                FirstName = Constants.TestFirstName,
                LastName = Constants.TestLecturerLastName,
                SchoolId = Constants.TestLecturerSchoolId,
                UserName = Constants.TestLecturerUserName,
                Role = Role.Lecturer,
            };
            
            var student = new ApplicationUser
            {
                FirstName = Constants.TestFirstName,
                LastName = Constants.TestStudentLastName,
                SchoolId = Constants.TestStudentSchoolId,
                UserName = Constants.TestStudentUserName,
                Role = Role.Student,
            };

            var result1 = await userManager.CreateAsync(admin, Constants.DefaultPassword);
            var result2 = await userManager.CreateAsync(lecturer, Constants.DefaultPassword);
            var result3 = await userManager.CreateAsync(student, Constants.DefaultPassword);

            if (result1.Succeeded && result2.Succeeded && result3.Succeeded)
            {
                await adminService.AddPreference(Constants.TestPreference1);
                await adminService.AddPreference(Constants.TestPreference2);
                await adminService.AddPreference(Constants.TestPreference3);

                return;
            }

            var errors = new List<IdentityError>();
            errors.AddRange(result1.Errors);
            errors.AddRange(result2.Errors);
            errors.AddRange(result3.Errors);

            throw new Exception(string.Join(",", errors.Select(x => x.Description)));

        }
    }
}
