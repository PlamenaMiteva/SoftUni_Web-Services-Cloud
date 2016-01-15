namespace Restaurants.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Restaurants.Data;
    using Restaurants.Models;
    using Restaurants.Services.Models.Binding_Models;
    using Restaurants.Services.Models.ViewModels;

    [Authorize]
    public class RestaurantsController : BaseApiController
    {
        public RestaurantsController(IRestaurantsData data) : base(data)
        {
        }

        //Unit Test Version
        //GET /api/restaurants?townId={townId}
        [AllowAnonymous]
        public IHttpActionResult GetRestaurants(
            [FromUri]SearchRestaurantsBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var town = this.Data.Towns.Find(model.TownId);
            if (town == null)
            {
                return this.NotFound();
            }

            var data = this.Data.Restaurants.All()
                .Where(r => r.TownId == model.TownId)
                .OrderByDescending(r => r.Ratings.Average(rt => rt.Stars))
                .ThenBy(r => r.Name)
                .Select(RestaurantViewModel.Create)
                .ToList();

            return this.Ok(data);
        }


        //GET /api/restaurants?townId={townId}
        //[HttpGet]
        //[AllowAnonymous]
        //public IHttpActionResult GetRestaurants(int townId)
        //{
        //    var town = this.Data.Towns.Find(townId);
        //    if (town == null)
        //    {
        //        return this.NotFound();
        //    }
        //    var data = this.Data.Restaurants.All()
        //        .Where(r => r.TownId == townId)
        //        .OrderByDescending(r => r.Ratings.Average(rt => rt.Stars))
        //        .ThenBy(r => r.Name)
        //        .Select(RestaurantViewModel.Create);

        //    return this.Ok(data);
        //}

        //POST /api/restaurants
        [HttpPost]
        public IHttpActionResult CreateRestaurant(
            [FromBody] CreateRestaurantBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null (no data in request)");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            string loggedUserId = this.User.Identity.GetUserId();

            var restaurant = new Restaurant()
            {
                Name = model.Name,
                TownId = model.TownId,
                OwnerId = loggedUserId
            };
            this.Data.Restaurants.Add(restaurant);
            this.Data.SaveChanges();

            var data = this.Data.Restaurants.All()
                .Where(r => r.Id == restaurant.Id)
                .Select(RestaurantViewModel.Create)
                .FirstOrDefault();

            return this.CreatedAtRoute(
                "DefaultApi",
                new { controller = "restaurants", id = restaurant.Id },
                data);
        }

        //POST /api/restaurants/{id}/rate
        [HttpPost]
        [Route("api/restaurants/{id}/rate")]
        public IHttpActionResult RateRestaurant(int id,
            [FromBody] RateBindingModel model)
        {
            var restaurant = this.Data.Restaurants.Find(id);
            if (restaurant == null)
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
            string loggedUserId = this.User.Identity.GetUserId();
            if (restaurant.OwnerId == loggedUserId)
            {
                return this.BadRequest("You cannot rate your own restaurant.");
            }
            var existingRating = this.Data.Ratings.All().FirstOrDefault(r=>r.UserId==loggedUserId && r.RestaurantId==id);
            if (existingRating == null)
            {
                var rating = new Rating()
                {
                    Stars = model.Stars,
                    RestaurantId = id,
                    UserId = loggedUserId
                };
                this.Data.Ratings.Add(rating);
            }
            else
            {
                existingRating.Stars = model.Stars;
            }
            this.Data.SaveChanges();
            return this.Ok();
        }

        
    }
}
