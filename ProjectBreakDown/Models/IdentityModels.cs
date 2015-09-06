using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;

namespace ProjectBreakDown.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Project> projects { get; set; }
        public virtual ICollection<ProjectTask> tasks { get; set; }
        public virtual ICollection<FriendRequest> friendRequestsReceived { get; set; }
        public virtual ICollection<FriendRequest> friendRequestsSent { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(p => p.members)
                .WithMany(m => m.projects)
                .Map(mc =>
                {
                    mc.ToTable("ProjectJoinMember");
                    mc.MapLeftKey("ProjectId");
                    mc.MapRightKey("UserId");
                });

            modelBuilder.Entity<ProjectTask>()
                .HasOptional(pt => pt.assignedTo)
                .WithMany(u => u.tasks);

            modelBuilder.Entity<ProjectTask>()
                .HasOptional(projTask => projTask.parentTask)
                .WithMany(parentTask => parentTask.subTasks);

            modelBuilder.Entity<FriendRequest>()
                .HasRequired(fr => fr.ToUser)
                .WithMany(tu => tu.friendRequestsReceived)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FriendRequest>()
                .HasRequired(fr => fr.FromUser)
                .WithMany(fu => fu.friendRequestsSent)
                .WillCascadeOnDelete(false);
                                

            base.OnModelCreating(modelBuilder);
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ProjectBreakDown.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<ProjectBreakDown.Models.ProjectTask> ProjectTasks { get; set; }

        public System.Data.Entity.DbSet<ProjectBreakDown.Models.FriendRequest> FriendRequests { get; set; }
    }
}