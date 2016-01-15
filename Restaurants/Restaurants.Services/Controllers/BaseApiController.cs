namespace Restaurants.Services.Controllers
{
    using System.Web.Http;
    using Restaurants.Data;

    public class BaseApiController : ApiController
    {
        //public BaseApiController()
        //    : this(new RestaurantsContext())
        //{
        //}

        //public BaseApiController(RestaurantsContext data)
        //{
        //    this.Data = data;
        //}

        //protected RestaurantsContext Data { get; set; }

        public BaseApiController(IRestaurantsData data)
        {
            this.Data = data;
        }

        public IRestaurantsData Data { get; set; }
    }
}
