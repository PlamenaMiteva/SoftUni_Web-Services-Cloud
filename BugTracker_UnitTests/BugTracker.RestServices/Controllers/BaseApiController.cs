using System.Web.Http;
using BugTracker.Data;
using BugTracker.Data.UnitOfWork;

namespace BugTracker.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new BugTrackerData(new BugTrackerDbContext()))
        {
        }

        public BaseApiController(IBugTrackerData data)
        {
            this.Data = data;
        }

        protected IBugTrackerData Data { get; set; }
    }
}
