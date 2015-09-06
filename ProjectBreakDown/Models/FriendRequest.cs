using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBreakDown.Models
{
    public class FriendRequest
    {

        public FriendRequest() { }
        public FriendRequest(ApplicationUser fromUser, ApplicationUser toUser)
        {
            this.FromUser = fromUser;
            this.ToUser = toUser;
            this.FromName = fromUser.UserName;
            this.ToName = toUser.UserName;
            this.status = "Pending approval";
            this.SentOn = DateTime.Now;
        }
        public int FriendRequestId { get; set; }
        public string FromName {get; set;}
        public string ToName { get; set; }
        public DateTime SentOn { get; set; }
        public string status { get; set; }
        [JsonIgnore]
        public ApplicationUser FromUser { get; set; }
        [JsonIgnore]
        public ApplicationUser ToUser { get; set; }

        public string AcceptRequest()
        {
            this.status = "Accepted";
            return this.status;
        }

        public string RejectRequest()
        {
            this.status = "Rejected";
            return this.status;
        }
        
    }
}