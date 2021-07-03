using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SchoolId { get; set; }

        public Role Role { get; set; }

        public virtual List<ProjectPreference> ProjectPreferences { get; set; }
    }
}
