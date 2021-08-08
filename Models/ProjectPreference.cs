using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectAllocationSystem.Models
{
    public class ProjectPreference
    {
        public int Id { get; set; }

        public string Preference { get; set; }

        [JsonIgnore]
        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
