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
    public class ChannelMessagesController : BaseApiController
    {

        public ChannelMessagesController(IMessagesData data)
            : base(data)
        {
        }
        //GET /api/channel-messages/{channelName}
        //[Route("api/channel-messages/{channelName}")]
        //public IHttpActionResult GetChannelMessages(string channelName)
        //{
        //    var channel = this.Data.Channels.FirstOrDefault(c => c.Name == channelName);
        //    if (channel==null)
        //    {
        //        return this.NotFound();
        //    }
        //    var channelMessages = channel.ChannelMessages.AsQueryable()
        //        .Select(ChannelMessagesViewModel.Create);
        //    return this.Ok(channelMessages);
        //}

        //GET /api/channel-messages/{channel}?limit={limit}
        [Route("api/channel-messages/{channel}")]
        public IHttpActionResult GetChannelMessagesWithLimit(string channel, [FromUri]LimitBindingModel model)
        {
            var dbChannel = this.Data.Channels.All().FirstOrDefault(c => c.Name == channel);
            if (dbChannel == null)
            {
                return this.NotFound();
            }
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var channelMessages = dbChannel.ChannelMessages.AsQueryable()
                .Take(model.Limit)
                .Select(ChannelMessagesViewModel.Create);
            return this.Ok(channelMessages);
        }

        //POST /api/channel-messages/{channelName}
        [HttpPost]
        [Route("api/channel-messages/{channelName}")]
        public IHttpActionResult SendAnonymousMessage(string channelName, [FromBody]ChannelMessagesBindingModel model)
        {
            var dbChannel = this.Data.Channels.All().FirstOrDefault(c => c.Name == channelName);
            if (dbChannel == null)
            {
                return this.NotFound();
            }
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var message = new ChannelMessage()
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                Channel = dbChannel,
                Sender = currentUser
            };
            this.Data.ChannelMessages.Add(message);
            this.Data.SaveChanges();
            if (currentUser == null)
            {
                return this.Ok(
                    new
                    {
                        message.Id,
                        Message = "Anonymous message sent successfully to channel " + channelName + "."
                    });
            }
            else
            {
                return this.Ok(
                    new
                    {
                        message.Id,
                        Sender = message.Sender.UserName,
                        Message = "Message sent to channel " + channelName + "."
                    });
            }
        }
    }
}
