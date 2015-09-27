namespace ProjectBreakDown.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendRequests",
                c => new
                    {
                        FriendRequestId = c.Int(nullable: false, identity: true),
                        FromName = c.String(),
                        ToName = c.String(),
                        SentOn = c.DateTime(nullable: false),
                        status = c.String(),
                        FromUser_Id = c.String(nullable: false, maxLength: 128),
                        ToUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.FriendRequestId)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUser_Id)
                .Index(t => t.FromUser_Id)
                .Index(t => t.ToUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        ProjectTaskId = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        assignedTo_Id = c.String(maxLength: 128),
                        parentTask_ProjectTaskId = c.Int(),
                        project_ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectTaskId)
                .ForeignKey("dbo.AspNetUsers", t => t.assignedTo_Id)
                .ForeignKey("dbo.ProjectTasks", t => t.parentTask_ProjectTaskId)
                .ForeignKey("dbo.Projects", t => t.project_ProjectId)
                .Index(t => t.assignedTo_Id)
                .Index(t => t.parentTask_ProjectTaskId)
                .Index(t => t.project_ProjectId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ProjectJoinMember",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.UserId })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.FriendRequests", "ToUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendRequests", "FromUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectTasks", "project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectTasks", "parentTask_ProjectTaskId", "dbo.ProjectTasks");
            DropForeignKey("dbo.ProjectTasks", "assignedTo_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectJoinMember", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectJoinMember", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ProjectJoinMember", new[] { "UserId" });
            DropIndex("dbo.ProjectJoinMember", new[] { "ProjectId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.ProjectTasks", new[] { "project_ProjectId" });
            DropIndex("dbo.ProjectTasks", new[] { "parentTask_ProjectTaskId" });
            DropIndex("dbo.ProjectTasks", new[] { "assignedTo_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.FriendRequests", new[] { "ToUser_Id" });
            DropIndex("dbo.FriendRequests", new[] { "FromUser_Id" });
            DropTable("dbo.ProjectJoinMember");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.Projects");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.FriendRequests");
        }
    }
}
