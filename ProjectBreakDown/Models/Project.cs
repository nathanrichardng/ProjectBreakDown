using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using ProjectBreakDown.Models;
using Newtonsoft.Json;

namespace ProjectBreakDown.Models
{
    public class Project
    {
        public Project()
        {
            this.members = new Collection<ApplicationUser>();
            this.tasks = new Collection<ProjectTask>();
        }
        public int ProjectId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUser> members { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProjectTask> tasks { get; set; }
    }
}