using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBreakDown.Models
{
    public class UserProfile
    {
        public int id { get; set; }
        public virtual ApplicationUser user { get; set; }
        public virtual ICollection<Project> projects { get; set; }
        public virtual ICollection<ProjectTask> tasks { get; set; }
        public virtual ICollection<FriendRequest> friendRequestsReceived { get; set; }
        public virtual ICollection<FriendRequest> friendRequestsSent { get; set; }
    }
}