using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Messages.Data;
using Messages.Data.Models;
using Messages.RestServices.Models.Binding_Models;
using Messages.RestServices.Models.View_Models;

namespace Messages.RestServices.Controllers
{
    public class ChannelsController : BaseApiController
    {
        public ChannelsController(IMessagesData data)
            : base(data)
        {
        }
        //GET /api/channels
        public IHttpActionResult GetChannels()
        {
            var channels = this.Data.Channels.All().OrderBy(c => c.Name).Select(ChannelViewModel.Create);
            return this.Ok(channels);
        }

        //GET /api/channels/{id}
        public IHttpActionResult GetChannelById(SearchChannelsBindingModel model)
        {
            var channel = this.Data.Channels.All().FirstOrDefault(c=>c.Id==model.ChannelId);
            if (channel==null)
            {
                return this.NotFound();
            }
            var result = this.Data.Channels.All().Where(c => c.Id == model.ChannelId).Select(ChannelViewModel.Create);
            return this.Ok(result);
        }

        //POST /api/channels
        [HttpPost]
        public IHttpActionResult CreateChannel([FromBody]CreateChannelBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var existingChannel = this.Data.Channels.All().FirstOrDefault(c => c.Name == model.Name);
            if (existingChannel!=null)
            {
                return this.Conflict();
            }
            var channel = new Channel()
            {
                Name = model.Name
            };
            this.Data.Channels.Add(channel);
            this.Data.SaveChanges();
            var result = this.Data.Channels.All().Where(c => c.Name == model.Name).Select(ChannelViewModel.Create);
            return this.CreatedAtRoute(
                "DefaultApi",
                new { controller = "channels", id = channel.Id },result);
        }

        //PUT /api/channels/{id}
        [HttpPut]
        public IHttpActionResult EditChannel(int id, [FromBody]CreateChannelBindingModel model)
        {   
            var channel = this.Data.Channels.Find(id);
            if (channel ==null)
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
            var existingChannel = this.Data.Channels.All().FirstOrDefault(c => c.Name == model.Name);
            if (existingChannel != null)
            {
                return this.Conflict();
            }
            channel.Name = model.Name;
            this.Data.SaveChanges();
            return this.Ok(string.Format("Channel #{0} edited successfully.", channel.Id));
        }

        //DELETE /api/channels/{id}
        [HttpDelete]
        public IHttpActionResult DeleteChannel(int id)
        {
            var channel = this.Data.Channels.Find(id);
            if (channel == null)
            {
                return this.NotFound();
            }
            if (channel.ChannelMessages.Any())
            {
                return this.Conflict();
            }
            this.Data.Channels.Delete(channel);
            this.Data.SaveChanges();
            return this.Ok(
                   new
                   {
                       Message = "Channel #" + channel.Id + " deleted."
                   });
        }
    }
}
