using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugTracker.Data.Models;
using BugTracker.Data.UnitOfWork;
using BugTracker.RestServices.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Controllers
{
    public class CommentsController : BaseApiController
    {
        public CommentsController(IBugTrackerData data)
            :base(data)
        {
                
        }
        public IHttpActionResult GetComments()
        {
            var data = this.Data.Comments.All()
                .OrderByDescending(c => c.CreatedOn)
                .Select(CommentsViewModel.Create);

            return this.Ok(data);
        }


        //GET /api/bugs/{id}/comments
        [Route("api/bugs/{id}/comments")]
        public IHttpActionResult GetBugComments(int id)
        {
            var bug = this.Data.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }
            var result = this.Data.Comments.All()
                .Where(c => c.BugId == id)
                .Select(CommentViewModel.Create);

            return this.Ok(result);
        }



        //POST /api/bugs/{id}/comments
        [HttpPost]
        [Route("api/bugs/{id}/comments")]
        public IHttpActionResult AddNewComment([FromBody]AddCommentBindingModel model, int id)
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
            var bug = this.Data.Bugs.Find(id);
            if (bug==null)
            {
                return this.NotFound();
            }   
            var comment = new Comment()
            {
                Text = model.Message,
                CreatedOn = DateTime.Now,
                CommentAuthor = user,
                Bug = bug
            };
            this.Data.Comments.Add(comment);
            this.Data.SaveChanges();
            if (user == null)
            {
                return this.Ok(new
                {
                    Id = comment.Id, 
                    Message = string.Format("Added anonymous comment for bug #{0}.", bug.Id)
                });
            }
            else
            {
                return this.Ok(new
                {
                    Id = comment.Id, 
                    Author = user.UserName, 
                    Message = string.Format("User comment added for bug #{0}.", bug.Id)
                });
            }
        }
    }
}
