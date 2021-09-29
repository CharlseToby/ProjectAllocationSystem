using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.ViewModels.Admin
{
    public class AllUsersVM
    {
        public IEnumerable<UserViewModel> AllUsers { get; set; }
    }

    public class UserViewModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string UserName { get; set; }
        
        public string Role { get; set; }
    }
}
