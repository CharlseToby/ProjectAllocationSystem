using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectAllocationSystem.Data;
using ProjectAllocationSystem.DTOs;
using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ILookupNormalizer _normalizer;

        public AdminService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
            IPasswordHasher<ApplicationUser> passwordHasher, ILookupNormalizer normalizer)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _normalizer = normalizer;
        }

        public async Task<OperationDataResult<ApplicationUser>> CreateUser(string firstName, string lastName, string schoolId, string role)
        {
            int appUsersCount = await _dbContext.Users.CountAsync();
            var user = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = $"{firstName}.{lastName}{schoolId}",
                SchoolId = schoolId,
                Role = (Role)Enum.Parse(typeof(Role), role)
            };
            var result = await _userManager.CreateAsync(user, Constants.DefaultPassword);

            if (result.Succeeded)
            {
                return new OperationDataResult<ApplicationUser>(user);
            }

            return new OperationDataResult<ApplicationUser>(string.Join(",", result.Errors));
        }

        public async Task<OperationDataResult<List<ApplicationUser>>> BulkCreateUsers(List<ApplicationUserDTO> applicationUsersDTO)
        {            
            var users = new List<ApplicationUser>();
            int appUsersCount = await _dbContext.Users.CountAsync();

            foreach (var userDTO in applicationUsersDTO)
            {
                users.Add(userDTO.ConvertToModel());
            }

            foreach (var user in users)
            {
                appUsersCount++;
                string securityStamp = Guid.NewGuid().ToString("N");
                user.UserName = $"{user.FirstName}.{user.LastName}{appUsersCount}".ToLower();
                user.NormalizedUserName = _normalizer.NormalizeName(user.UserName);
                string password = Constants.DefaultPassword;
                user.SecurityStamp = securityStamp;
                string passwordHash = _passwordHasher.HashPassword(user, password);
                user.PasswordHash = passwordHash;
            }

            try
            {
                await _dbContext.Users.AddRangeAsync(users);
                await _dbContext.SaveChangesAsync();
                return new OperationDataResult<List<ApplicationUser>>(users);
            }
            catch
            {
                return new OperationDataResult<List<ApplicationUser>>("Invalid or Duplicate Data Entered");
            }
        }

        public async Task<OperationResult> AddPreference(string preference)
        {
            if (_dbContext.ProjectPreferences.FirstOrDefault(x => x.Preference == preference.ToUpper()) is null)
            {
                _dbContext.ProjectPreferences.Add(new ProjectPreference
                {
                    Preference = preference.ToUpper()
                });
                await _dbContext.SaveChangesAsync();
                return new OperationResult(true);
            }
            return new OperationResult("Duplicate entries not allowed");
        }

        public async Task<OperationResult> RemovePreference(string preference)
        {
            var pref = _dbContext.ProjectPreferences.FirstOrDefault(x => x.Preference == preference);
            if (pref.ApplicationUsers.Count < 1)
            {
                _dbContext.ProjectPreferences.Remove(pref);
                await _dbContext.SaveChangesAsync();
            }
            return new OperationResult("Users are already have this preference");
        }
    }
}
