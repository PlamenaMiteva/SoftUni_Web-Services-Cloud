using System;
using System.Linq;
using System.Web.Http;
using Messages.Data;
using Messages.Data.Models;
using Messages.RestServices.Models.Binding_Models;
using Messages.RestServices.Models.View_Models;
using Microsoft.AspNet.Identity;

namespace Messages.RestServices.Controllers
{
    public class UserController : BaseApiController
    {
        public UserController(IMessagesData data)
            : base(data)
        {
        }
        //GET /api/user/personal-messages
        [Route("api/user/personal-messages")]
        [Authorize]
        public IHttpActionResult GetPersonalMessages()
        {
            var currentUserId = User.Identity.GetUserId();
            var messages = this.Data.UserMessages.All().Where(m => m.RecieverId == currentUserId)
                .Select(UserMessagesViewModel.Create);
            return this.Ok(messages);
        }

        //POST /api/user/personal-messages
        [Route("api/user/personal-messages")]
        [HttpPost]
        public IHttpActionResult SendAnonymousPersonalMessage([FromUri]UserMessagesBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var reciever = this.Data.Users.All().FirstOrDefault(u => u.UserName == model.Recipient);
            if (reciever==null)
            {
                return this.NotFound();
            }
            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var message = new UserMessage()
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                Reciever = reciever,
                Sender = currentUser
            };
            this.Data.UserMessages.Add(message);
            this.Data.SaveChanges();
            if (currentUser == null)
            {
                return this.Ok(
                    new
                    {
                        message.Id,
                        Message = "Anonymous message successfully sent to user " + model.Recipient + "."
                    });

            }
            else
            {
                return this.Ok(
                     new
                     {
                         message.Id,
                         Sender = message.Sender.UserName,
                         Message = "Message successfully sent to user " + model.Recipient + "."
                     }); 
            }
        }
    }
}
