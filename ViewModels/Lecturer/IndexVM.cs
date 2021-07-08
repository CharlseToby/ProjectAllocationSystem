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
        public List<WaitingStudent> WaitingStudents { get; set; }
    }

    public record WaitingStudent
    {
        public ApplicationUser Student { get; set; }

        public List<string> PreferenceMatch { get; set; }
    }
}
