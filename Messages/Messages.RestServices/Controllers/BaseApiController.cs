using System.Web.Http;
using Messages.Data;

namespace Messages.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        //public BaseApiController()
        //    : this(new MessagesDbContext())
        //{
        //}

        //public BaseApiController(MessagesDbContext data)
        //{
        //    this.Data = data;
        //}
        //protected MessagesDbContext Data { get; set; }

        public BaseApiController(IMessagesData data)
        {
            this.Data = data;
        }

        public IMessagesData Data { get; set; }
    }
}
