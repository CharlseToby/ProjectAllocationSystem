﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectAllocationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectPreference> ProjectPreferences { get; set; }

        public DbSet<LecturerStudentNode> LecturerStudentNodes { get; set; }
    }
}