using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectBreakDown.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace ProjectBreakDown.Models
{
    public class ProjectTask
    {
        public ProjectTask()
        {
            this.subTasks = new Collection<ProjectTask>();
        }
        public int ProjectTaskId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser assignedTo { get; set; }
        [JsonIgnore]
        public virtual Project project { get; set; }
        [JsonIgnore]
        public virtual ProjectTask parentTask { get; set; }
        public virtual ICollection<ProjectTask> subTasks { get; set; }
    }
}