using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.DTOs
{
    public class ApplicationUserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SchoolId { get; set; }

        public string Role { get; set; }

        public ApplicationUser ConvertToModel()
        {
            var user = new ApplicationUser
            {
                FirstName = FirstName,
                LastName = LastName,
                SchoolId = SchoolId,
                Role = (Role) Enum.Parse(typeof(Role), Role) 
            };

            return user;
        }
    }
}
