using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.ViewModels.Admin
{
    public record IndexVM
    {
        public List<string> Preferences { get; set; }
    }
}
