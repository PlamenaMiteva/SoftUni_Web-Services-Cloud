using System.Web.Http;
using CriminalActivities.Data;

namespace CriminalActivities.App.Controllers
{
    public class BaseApiController : ApiController
    {
        //public BaseApiController()
        //    : this(new CriminalContext())
        //{
        //}

        //public BaseApiController(CriminalContext data)
        //{
        //    this.Data = data;
        //}

        //protected CriminalContext Data { get; set; }

        public BaseApiController(ICriminalActivitiesData data)
        {
            this.Data = data;
        }

        public ICriminalActivitiesData Data { get; set; }
    }
}
