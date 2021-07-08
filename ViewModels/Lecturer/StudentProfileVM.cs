using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.ViewModels.Lecturer
{
    public class StudentProfileVM
    {
        public ApplicationUser ApplicationUser { get; set; }

        public bool AssignedToLecturer { get; set; }

    }
}
