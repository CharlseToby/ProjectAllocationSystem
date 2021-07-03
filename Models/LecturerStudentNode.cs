using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Models
{
    public class LecturerStudentNode
    {
        public int Id { get; set; }

        public int LecturerId { get; set; }

        public int StudentId { get; set; }

        public List<string> Chat { get; set; }
    }
}
