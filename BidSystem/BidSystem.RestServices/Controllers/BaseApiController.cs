using System.Web.Http;
using BidSystem.Data;

namespace BidSystem.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new BidSystemDbContext())
        {
        }

        public BaseApiController(BidSystemDbContext data)
        {
            this.Data = data;
        }

        protected BidSystemDbContext Data { get; set; }
    }
}
