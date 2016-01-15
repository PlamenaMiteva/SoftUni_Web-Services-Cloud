using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Models.Binding_Models;
using Restaurants.Services.Models.ViewModels;

namespace Restaurants.Services.Controllers
{
    [Authorize]
    public class MealsController : BaseApiController
    {
        public MealsController(IRestaurantsData data)
            : base(data)
        {
        }
        //GET /api/restaurants/{id}/meals
        [HttpGet]
        [AllowAnonymous]
        [Route("api/restaurants/{id}/meals")]
        public IHttpActionResult GetRestaurants(int id)
        {
            var restaurant = this.Data.Restaurants.Find(id);
            if (restaurant == null)
            {
                return this.NotFound();
            }
            var data = this.Data.Meals.All()
                .Where(m => m.RestaurantId == id)
                .OrderBy(m => m.TypeId)
                .ThenBy(m => m.Name)
                .Select(MealViewModel.Create);

            return this.Ok(data);
        }


        //POST /api/meals
        [HttpPost]
        public IHttpActionResult CreateMeal(
            [FromBody] CreateMealBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            string loggedUserId = this.User.Identity.GetUserId();
            var restaurant = this.Data.Restaurants.Find(model.RestaurantId);
            var type = this.Data.MealTypes.Find(model.TypeId);
            if (type==null)
            {
                return this.BadRequest("Type with this id does not exist");
            }
            if (restaurant == null)
            {
                return this.BadRequest("Restaurant with this id does not exist");
            }
            if (loggedUserId!= restaurant.OwnerId)
            {
                return this.Unauthorized();
            }
            var meal = new Meal()
            {
                Name = model.Name,
                Price = model.Price,
                TypeId = model.TypeId,
                RestaurantId = model.RestaurantId
            };
            this.Data.Meals.Add(meal);
            this.Data.SaveChanges();

            var data = this.Data.Meals.All()
                .Where(m => m.Id == meal.Id)
                .Select(MealViewModel.Create)
                .FirstOrDefault();

            return this.CreatedAtRoute(
                "DefaultApi",
                new { controller = "meals", id = meal.Id },
                data);
        }


        //PUT /api/meals/{id}
        [HttpPut]
        public IHttpActionResult EditMeal(int id,
            [FromBody] EditMealBindingModel model)
        { 
            var meal = this.Data.Meals.Find(id);
            if (meal == null)
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
            if (loggedUserId != meal.Restaurant.OwnerId)
            {
                return this.Unauthorized();
            }
            meal.Name = model.Name;
            meal.Price = model.Price;
            meal.TypeId = model.TypeId;
            this.Data.SaveChanges();

            var data = this.Data.Meals.All()
                .Where(m => m.Id == meal.Id)
                .Select(MealViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }


        //DELETE /api/meals/{id}
        [HttpDelete]
        public IHttpActionResult DeleteMeal(int id)
        {
            var meal = this.Data.Meals.Find(id);
            if (meal == null)
            {
                return this.NotFound();
            }
            string loggedUserId = this.User.Identity.GetUserId();
            if (loggedUserId != meal.Restaurant.OwnerId)
            {
                return this.Unauthorized();
            }
            this.Data.Meals.Delete(meal);
            this.Data.SaveChanges();

            return this.Ok(string.Format("Meal #{0} deleted.", id));
        }

        
    }
}
