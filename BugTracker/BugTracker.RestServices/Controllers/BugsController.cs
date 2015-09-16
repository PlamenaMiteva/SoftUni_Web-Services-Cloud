using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.WebPages;
using BugTracker.Data.Models;
using BugTracker.RestServices.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Controllers
{
    [RoutePrefix("api/bugs")]
    public class BugsController : BaseApiController
    {
        // GET api/bugs
        public IHttpActionResult GetBugs()
        {
            var data = this.Data.Bugs
                .OrderByDescending(b => b.SubmitDate)
                .Select(BugsViewModel.Create);

            return this.Ok(data);
        }

        // GET api/bugs/{id}
        [Route("{id}")]
        public IHttpActionResult GetBugById(int id)
        {
            var bug = this.Data.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }
            var result = this.Data.Bugs
                .Where(b => b.Id == id)
                .Select(BugsViewModel.Create);

            return this.Ok(result);
        }

        // POST api/bugs
        [HttpPost]
        public IHttpActionResult AddNewBug([FromBody]AddBugBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            string loggedUserId = this.User.Identity.GetUserId();
            var user = this.Data.Users.Find(loggedUserId);
            var bug = new Bug()
            {
                Title = model.Title,
                Description = model.Description,
                Status = Status.Open,
                SubmitDate = DateTime.Now,
                BugAuthor = user
            };
            this.Data.Bugs.Add(bug);
            this.Data.SaveChanges();
            if (user == null)
            {
                return this.CreatedAtRoute("DefaultApi", new { id = bug.Id },
                    new { id = bug.Id, message = "Annonymous bug submitted." });
            }
            else
            {
                return this.CreatedAtRoute("DefaultApi", new { id = bug.Id },
                    new { id = bug.Id, author = user.UserName, message = "User bug submitted." });
            }
        }

        //PATCH /api/bugs/{id}
        [HttpPatch]
        [Route("{id}")]
        public IHttpActionResult EditBug([FromBody]EditBugBindingModel model, int id)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var bug = this.Data.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }
            if (model.Title != null)
            {
                bug.Title = model.Title;
            }
            if (model.Description != null)
            {
                bug.Description = model.Description;
            }
            if (model.Status != null)
            {
                Status newStatus;
                bool isSuccessful = Enum.TryParse(model.Status, out newStatus);
                if (!isSuccessful)
                {
                    return this.BadRequest("Invalid bug status.");
                }
                bug.Status = newStatus;
            }
            this.Data.SaveChanges();
            return this.Ok("Bug #" + bug.Id + " patched.");
        }

        //DELETE /api/bugs/{id}
        [Route("{id}")]
        public IHttpActionResult DeleteBug(int id)
        {
            var bug = this.Data.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }
            this.Data.Bugs.Remove(bug);
            this.Data.SaveChanges();
            return this.Ok("Bug #" + bug.Id + " deleted.");
        }

        //GET /api/bugs/filter keyword={keyword}&statuses={status1|status2|…}&author={author-username}
        [Route("filter")]
        public IHttpActionResult GetAllBugsMatchingAFilter([FromUri]FilterBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null.");
            }
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var bugs = this.Data.Bugs.AsQueryable();
            if (model.Keyword != null)
            {
                bugs = bugs.Where(b => b.Title.Contains(model.Keyword));
            }
            if (model.AuthorUsername != null)
            {
                bugs = bugs.Where(b => b.BugAuthor.UserName.Contains(model.AuthorUsername));
            }
            if (model.Statuses != null)
            {
                var bugStatuses = new List<Status>();
                var input = model.Statuses.Split('|');
                foreach (var status in input)
                {
                    Status parsedStatus;
                    bool isSuccessful = Enum.TryParse(status, out parsedStatus);
                    if (isSuccessful)
                    {
                        bugStatuses.Add(parsedStatus);
                    }
                    bugs = bugs.Where(b => bugStatuses.Contains(b.Status));
                }
            }
            var data = bugs.OrderByDescending(b => b.SubmitDate).Select(BugsViewModel.Create);
            return this.Ok(data);
        }

        
    }
}

