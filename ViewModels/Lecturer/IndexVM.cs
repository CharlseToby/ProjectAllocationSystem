using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.ViewModels.Lecturer
{
    public record IndexVM
    {                
        public List<AssignedStudent> AssignedStudents { get; set; }
    }

    public record AssignedStudent
    {
        public ApplicationUser Student { get; set; }

        public List<string> PreferenceMatch { get; set; }
    }
}
