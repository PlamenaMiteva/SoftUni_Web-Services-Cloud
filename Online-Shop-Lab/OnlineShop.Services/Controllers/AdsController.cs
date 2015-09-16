using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Infrastructure;
using OnlineShop.Services.Models;
using OnlineShop.Services.Models.ViewModels;

namespace OnlineShop.Services.Controllers
{
    [RoutePrefix("api/ads")]
    public class AdsController : BaseApiController
    {
        public AdsController()
            : base()
        {
        }
        public AdsController(IOnlineShopData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        public IHttpActionResult GetAds()
        {
            var ads = this.Data.Ads.All()
                .Where(a => a.Status == AdStatus.Open)
                .OrderByDescending(a => a.Type.Index)
                .ThenByDescending(a => a.PostedOn)
                .Select(AdViewModel.Create);
            return this.Ok(ads);
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateAd(CreateAdBindingModel model)
        {
            var userId = this.UserIdProvider.GetUserId();
            var owner = this.Data.Users.Find(userId);
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var ad = new Ad()
            {
                Name=model.Name,
                Description=model.Description,
                Type = this.Data.AdTypes.Find(model.TypeId),
                Price = model.Price,
                PostedOn = DateTime.Now,
                Owner = owner,
                Categories= new HashSet<Category>()
            };
            if (model.Categories == null)
            {
                return this.BadRequest("You should enter at least one category!");
            }
            foreach (var id in model.Categories)
            {
                var category = this.Data.Categories.Find(id);
                if (category==null)
                {
                    return NotFound();
                }
                ad.Categories.Add(category);
            }
            
            if (ad.Categories.Count > 3)
            {
                return this.BadRequest("The number of categories should not exceed 3!");
            }
            this.Data.Ads.Add(ad);
            this.Data.SaveChanges();
            var result = this.Data.Ads.All()
                .Where(a => a.Id == ad.Id)
                .Select(AdViewModel.Create)
                .FirstOrDefault();
            return this.Ok(result);
        }

        [Authorize]
        [HttpPut]
        [Route("{id}/close")]
        public IHttpActionResult CloseAd(int id)
        {
            Ad ad = this.Data.Ads.Find(id);
            if (ad==null)
            {
               return BadRequest("Sorry, the ad does not exist!"); 
            }
            var userId = this.UserIdProvider.GetUserId();
            if (ad.OwnerId!=userId)
            {
                return BadRequest("Sorry, you are not authorized to change this ad!"); 
            }
            ad.ClosedOn = DateTime.Now;
            ad.Status = AdStatus.Closed;
            this.Data.SaveChanges();
            var result = this.Data.Ads.All()
                .Where(a => a.Id == ad.Id)
                .Select(AdViewModel.Create)
                .FirstOrDefault();
            return this.Ok(result);
        }
    }
}
