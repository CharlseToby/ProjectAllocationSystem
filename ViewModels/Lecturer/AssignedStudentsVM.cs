using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.ViewModels.Lecturer
{
    public record AssignedStudentsVM
    {
        public List<ApplicationUser> AssignedStudents { get; set; }
    }
}
