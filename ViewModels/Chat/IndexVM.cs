using ProjectAllocationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.ViewModels.Chat
{
    public class IndexVM
    {
        public int NodeId { get; set; }

        public string LecturerName { get; set; }

        public string StudentName { get; set; }

        public Role UserRole { get; set; }
    }
}
