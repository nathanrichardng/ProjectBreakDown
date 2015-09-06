namespace ProjectBreakDown.Migrations
{
    using ProjectBreakDown.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjectBreakDown.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProjectBreakDown.Models.ApplicationDbContext context)
        {
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var user = new ApplicationUser { Email = "trybiteme@yahoo.com", UserName = "trybiteme@yahoo.com" };
            var user2 = new ApplicationUser { Email = "playcracktheskyy@gmail.com", UserName = "playcracktheskyy@gmail.com" };
            var user3 = new ApplicationUser { Email = "testing@gmail.com", UserName = "testing@gmail.com" };
            manager.Create(user, "Testing1!");
            manager.Create(user2, "Testing1!");
            manager.Create(user3, "Testing1!");

            var project = new Project { ProjectId = 1, name = "Project 1", description = "first project" };
            var project2 = new Project { ProjectId = 2, name = "Project 2", description = "second project" };

            project.members.Add(user);
            project2.members.Add(user2);

            var task = new ProjectTask { ProjectTaskId = 1, name = "task 1", description = "first task", assignedTo = user, project = project };
            var task2 = new ProjectTask { ProjectTaskId = 2, name = "task 2", description = "second task", assignedTo = user2, project = project2 };
            var task3 = new ProjectTask { ProjectTaskId = 3, name = "task 3", description = "third task, child of task 2", assignedTo = user2, project = project2 };

            task2.subTasks.Add(task3);

            var friendRequest = new FriendRequest(user2, user);
            var friendRequest2 = new FriendRequest(user3, user);

            context.FriendRequests.Add(friendRequest);
            context.FriendRequests.Add(friendRequest2);
            context.Projects.Add(project);
            context.Projects.Add(project2);
            context.ProjectTasks.Add(task);
            context.ProjectTasks.Add(task2);
            context.SaveChanges();
        }
    }
}
