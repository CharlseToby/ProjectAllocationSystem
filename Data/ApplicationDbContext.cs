using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {     
        }

        public DbSet<ProjectPreference> ProjectPreferences { get; set; }

        public DbSet<LecturerStudentNode> LecturerStudentNodes { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LecturerStudentNode>()
                .HasIndex(x => x.StudentId)
                .IsUnique();
        }
    }
}