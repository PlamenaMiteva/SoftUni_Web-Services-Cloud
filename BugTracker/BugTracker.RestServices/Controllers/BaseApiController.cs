using System.Web.Http;
using BugTracker.Data;

namespace BugTracker.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new BugTrackerDbContext())
        {
        }

        public BaseApiController(BugTrackerDbContext data)
        {
            this.Data = data;
        }

        protected BugTrackerDbContext Data { get; set; }
    }
}
