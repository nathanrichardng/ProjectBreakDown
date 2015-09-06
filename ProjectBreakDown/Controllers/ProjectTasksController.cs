using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectBreakDown.Models;

namespace ProjectBreakDown.Controllers
{
    public class ProjectTasksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        //next step is to map out how to assign projecttask to project. should be done through semantic route.
        //also want to add logic so that a user can only modify their own projectTasks.

        // GET: api/ProjectTasks
        public ICollection<ProjectTask> GetProjectTasks()
        {
            var user = db.Users.First(u => u.UserName == User.Identity.Name);
            return user.tasks;
        }

        // GET: api/ProjectTasks/5
        [ResponseType(typeof(ProjectTask))]
        public IHttpActionResult GetProjectTask(int id)
        {
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            if (projectTask == null)
            {
                return NotFound();
            }

            return Ok(projectTask);
        }

        // PUT: api/ProjectTasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProjectTask(int id, ProjectTask projectTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectTask.ProjectTaskId)
            {
                return BadRequest();
            }

            db.Entry(projectTask).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProjectTasks
        [ResponseType(typeof(ProjectTask))]
        public IHttpActionResult PostProjectTask(ProjectTask projectTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProjectTasks.Add(projectTask);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = projectTask.ProjectTaskId }, projectTask);
        }

        // DELETE: api/ProjectTasks/5
        [ResponseType(typeof(ProjectTask))]
        public IHttpActionResult DeleteProjectTask(int id)
        {
            ProjectTask projectTask = db.ProjectTasks.Find(id);
            if (projectTask == null)
            {
                return NotFound();
            }

            db.ProjectTasks.Remove(projectTask);
            db.SaveChanges();

            return Ok(projectTask);
        }

        //GET: api/Projects/5/ProjectTasks
        [Route("api/Projects/{projectId}/ProjectTasks")]
        [HttpGet]
        public ICollection<ProjectTask> FindTasksByProject(int projectId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var tasks = user.tasks.Where(t => t.project.ProjectId == projectId);
            return tasks.ToList();
        }

        //POST: api/Projects/5/ProjectTasks
        [Route("api/Projects/{projectId}/ProjectTasks")]
        [ResponseType(typeof(ProjectTask))]
        [HttpPost]
        public IHttpActionResult AddTaskToProject(int projectId, ProjectTask projectTask)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var project = user.projects.FirstOrDefault(p => p.ProjectId == projectId);
            if (user == null || project == null)
            {
                return BadRequest();
            }
            user.tasks.Add(projectTask);
            project.tasks.Add(projectTask);
            
            db.SaveChanges();
            return Ok(projectTask);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectTaskExists(int id)
        {
            return db.ProjectTasks.Count(e => e.ProjectTaskId == id) > 0;
        }
    }
}