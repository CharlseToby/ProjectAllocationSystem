using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.ViewModels.Student
{
    public record IndexVM
    {
        public string FirstName { get; set; }

        public List<ProjectPreference> StudentPreferences { get; set; }

        public List<ProjectPreference> AllPreferences { get; set; }

        public string SupervisorId { get; set; } = null;

        public string SupervisorFirstName { get; set; } = null;

        public string SupervisorLastName { get; set; } = null;

        public bool HasPreference()
        {
            return StudentPreferences.Count > 0;
        }

        public bool HasSupervisor()
        {
            return SupervisorId is not null;
        }
    }
}
