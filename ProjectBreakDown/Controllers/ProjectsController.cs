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
    public class ProjectsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Projects
        public ICollection<Project> GetProjects()
        {
            var user = db.Users.First(u => u.UserName == User.Identity.Name);
            return user.projects;
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            Project project = user.projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var proj = user.projects.FirstOrDefault(p => p.ProjectId == id);

            if (id != project.ProjectId || proj == null)
            {
                return BadRequest();
            }

            proj.name = project.name;
            proj.description = project.description;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = db.Users.First(u => u.UserName == User.Identity.Name);
            project.members.Add(user);
            db.Projects.Add(project);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(int id)
        {
            var user = db.Users.First(u => u.UserName == User.Identity.Name);
            Project project = user.projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectId == id) > 0;
        }
    }
}