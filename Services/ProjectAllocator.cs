using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectAllocationSystem.Data;
using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Services
{
    public class ProjectAllocator : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public ProjectAllocator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        // Project Allocation job runs every 30 minutes
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async o =>
            {
                await AllocateStudentsToLectures();
            },
        null,
        TimeSpan.Zero,
        TimeSpan.FromMinutes(30));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        async Task AllocateStudentsToLectures()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                if (configuration.GetValue<bool>("DB_MIGRATED", true))
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var assignedStudentsId = dbContext.LecturerStudentNodes.Select(x => x.StudentId).ToList();
                    var unassignedStudents = dbContext.Users
                        .Where(x => x.Role == Role.Student)
                        .Where(x => !assignedStudentsId.Contains(x.Id))
                        .ToList();
                    var lecturers = dbContext.Users
                        .Where(x => x.Role == Role.Lecturer)
                        .ToList();

                    var lecturerStudentNodes = new List<LecturerStudentNode>();
                    var random = new Random();

                    for (int i = 0; i < unassignedStudents.Count; i++)
                    {
                        var potentialSupervisors = lecturers.Where(x => x.ProjectPreferences.Any(y => unassignedStudents[i].ProjectPreferences.Contains(y)))
                            .ToList();
                        if (potentialSupervisors.Any())
                        {
                            int supervisorIndex = random.Next(0, potentialSupervisors.Count() - 1);
                            var node = new LecturerStudentNode
                            {
                                LecturerId = potentialSupervisors[supervisorIndex].Id,
                                StudentId = unassignedStudents[i].Id,
                                Chat = new()
                            };
                            lecturerStudentNodes.Add(node);
                        }
                    }
                    await dbContext.LecturerStudentNodes.AddRangeAsync(lecturerStudentNodes);
                    await dbContext.SaveChangesAsync();
                }                
            }
        }
    }
}
