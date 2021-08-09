using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAllocationSystem.Data;
using ProjectAllocationSystem.Models;
using ProjectAllocationSystem.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int nodeId)
        {
            var node = await _dbContext.LecturerStudentNodes.FindAsync(nodeId);

            var user = await _userManager.GetUserAsync(User);
            var lecturer = await _userManager.FindByIdAsync(node.LecturerId);
            var student = await _userManager.FindByIdAsync(node.StudentId);

            var vm = new IndexVM
            {
                NodeId = nodeId,
                LecturerName = $"{lecturer.FirstName} {lecturer.LastName}",
                StudentName = $"{student.FirstName} {student.LastName}",
                UserRole = user.Role
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetChats(int nodeId)
        {
            var chat = (await _dbContext.LecturerStudentNodes.FindAsync(nodeId)).Chat;
            return Ok(chat);
        }
        
        [HttpPost]
        public async Task<IActionResult> SendChat(int nodeId, string message)
        {
            var user = await _userManager.GetUserAsync(User);
            string prefix = "";

            switch (user.Role)
            {
                case Role.Lecturer:
                    prefix = Constants.LecturerChatPrefix;
                    break;

                case Role.Student:
                    prefix = Constants.StudentChatPrefix;
                    break;

                default:
                    throw new Exception("Invalid User");
            }

            var node = await _dbContext.LecturerStudentNodes.FindAsync(nodeId);
            node.Chat.Add($"{prefix}{message}");
            await _dbContext.SaveChangesAsync();
            return Ok();
        }        
    }
}
