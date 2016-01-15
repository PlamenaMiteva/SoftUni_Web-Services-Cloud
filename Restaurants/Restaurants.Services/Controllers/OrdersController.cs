namespace Restaurants.Services.Controllers
{
    using System;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Restaurants.Models;
    using Models.Binding_Models;
    using Models.ViewModels;
    using Restaurants.Data;
    using System.Linq;

    [Authorize]
    public class OrdersController : BaseApiController
    {
        public OrdersController(IRestaurantsData data)
            : base(data)
        {
        }
        //POST /api/meals/{id}/order
        [HttpPost]
        [Route("api/meals/{id}/order")]
        public IHttpActionResult CreateOrder(int id, [FromBody] CreateOrderBindingModel model)
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
            var order = new Order()
            {
                Quantity = (int)model.Quantity,
                MealId = id,
                UserId = loggedUserId,
                CreatedOn = DateTime.Now,
                OrderStatus = OrderStatus.Delivered
            };

            this.Data.Orders.Add(order);
            this.Data.SaveChanges();

            return this.Ok();
        }

        //GET /api/orders?startPage={start-page}&limit={page-size}&mealId={mealId}
        public IHttpActionResult GetOrders([FromUri]GetOrdersBindingModel model)
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
            if (model.MealId.HasValue)
            {
                var orders = this.Data.Orders.All()
                    .Where(o => o.UserId == loggedUserId && o.OrderStatus == OrderStatus.Pending && o.MealId == model.MealId)
                    .OrderByDescending(o => o.CreatedOn)
                    .Skip(model.StartPage * model.Limit)
                    .Take(model.Limit)
                    .Select(OrderViewModel.Create);
                return this.Ok(orders);
            }
            else
            {
                var orders = this.Data.Orders.All()
                    .Where(o => o.UserId == loggedUserId && o.OrderStatus == OrderStatus.Pending)
                    .OrderByDescending(o => o.CreatedOn)
                    .Skip(model.StartPage * model.Limit)
                    .Take(model.Limit)
                    .Select(OrderViewModel.Create);
                return this.Ok(orders);
            }
        }


    }
}
