using System;
using System.Linq;
using System.Web.Http;
using BidSystem.Data.Models;
using BidSystem.RestServices.Models;
using Microsoft.AspNet.Identity;

namespace BidSystem.RestServices.Controllers
{
    public class OffersController : BaseApiController
    {
        // GET /api/offers/all
        [HttpGet]
        [Route("api/offers/all")]
        public IHttpActionResult ExtractAllOffers()
        {

            var data = this.Data.Offers
                .OrderByDescending(o => o.PublishDate)
                .Select(OffersViewModel.Create);

            return this.Ok(data);
        }

        // GET /api/offers/active
        [HttpGet]
        [Route("api/offers/active")]
        public IHttpActionResult ExtractAllActiveOffers()
        {

            var data = this.Data.Offers
                .Where(o => o.ExpirationhDate > DateTime.Now)
                .OrderBy(o => o.ExpirationhDate)
                .Select(OffersViewModel.Create);

            return this.Ok(data);
        }

        // GET /api/offers/expired
        [HttpGet]
        [Route("api/offers/expired")]
        public IHttpActionResult ExtractAllexpiredOffers()
        {

            var data = this.Data.Offers
                .Where(o => o.ExpirationhDate < DateTime.Now)
                .OrderBy(o => o.ExpirationhDate)
                .Select(OffersViewModel.Create);

            return this.Ok(data);
        }

        // GET /api/offers/details/{id}
        [Route("api/offers/details/{id}")]
        public IHttpActionResult GetOfferById(int id)
        {
            var offer = this.Data.Offers.Find(id);
            if (offer == null)
            {
                return this.NotFound();
            }
            var data = this.Data.Offers
                .Where(o => o.Id == id)
                .Select(OfferViewModel.Create);

            return this.Ok(data);
        }

        // GET /api/offers/offers/my
        [Route("api/offers/offers/my")]
        [Authorize]
        public IHttpActionResult GetUsersOffers()
        {
            var loggedUserId = User.Identity.GetUserId();
            //if (loggedUserId==null)
            //{
            //    return this.Unauthorized();
            //}
            var offers = this.Data.Offers
                .Where(o => o.SellerId == loggedUserId)
                .Select(OffersViewModel.Create);

            return this.Ok(offers);
        }


        // POST /api/offers
        [Route("api/offers")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateNewOffer([FromBody]CreateNewOfferBindingModel model)
        {
            var loggedUserId = User.Identity.GetUserId();
            var user = this.Data.Users.Find(loggedUserId);
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }
            if (model == null)
            {
                return this.BadRequest();
            }
            var newOffer = new Offer()
            {
                Title = model.Title,
                Description = model.Description,
                InitialPrice = model.InitialPrice,
                ExpirationhDate = model.ExpirationDate,
                PublishDate = DateTime.Now,
                SellerId = loggedUserId
            };
            this.Data.Offers.Add(newOffer);
            this.Data.SaveChanges();
            return this.CreatedAtRoute("OfferDetails", new { id = newOffer.Id },
                    new { id = newOffer.Id, Seller = user.UserName, message = "Offer created." });
        }


        // GET /api/offers/bids/my
        [Route("api/offers/bids/my")]
        [Authorize]
        public IHttpActionResult GetUsersBids()
        {
            var loggedUserId = User.Identity.GetUserId();
            var bids = this.Data.Bids
                .Where(b => b.BidderId == loggedUserId)
                .OrderByDescending(b => b.BidDate)
                .Select(BidsViewModel.Create);

            return this.Ok(bids);
        }

        // GET /api/offers/bids/won
        [Route("api/offers/bids/won")]
        [Authorize]
        public IHttpActionResult GetUsersWonBids()
        {
            var loggedUserId = User.Identity.GetUserId();
            var bids = this.Data.Offers
                .Select(o => o.Bids
                    .OrderByDescending(b => b.BidPrice)
                    .FirstOrDefault());
            var wonBids = bids
                .Where(b=>b.BidderId==loggedUserId)
                .OrderBy(b=>b.BidDate)
                .Select(BidsViewModel.Create);

            return this.Ok(wonBids);
        }

        // GET /api/offers/{id}/bid
        [Route("api/offers/{id}/bid")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult AddNewBid(int id, [FromBody]AddBidBindingModel model)
        {
            var loggedUserId = User.Identity.GetUserId();
            var user = this.Data.Users.Find(loggedUserId);
            var offer = this.Data.Offers.Find(id);
            if (offer == null)
            {
                return this.NotFound();
            }
            if (!this.ModelState.IsValid || model == null)
            {
                return this.BadRequest();
            }
            if (offer.ExpirationhDate < DateTime.Now)
            {
                return this.BadRequest("Offer has expired.");
            }
            var maxBidPrice = offer.Bids.Where(b => b.BidPrice > model.BidPrice).Max(b=>b.BidPrice);
            if (model.BidPrice <= offer.InitialPrice)
            {
                return this.BadRequest(string.Format("Your bid should be > {0}.", offer.InitialPrice));
            }
            if (maxBidPrice>0)
            {
                return this.BadRequest(string.Format("Your bid should be > {0}.", maxBidPrice));
            }
            var newBid = new Bid()
            {
                BidPrice = model.BidPrice,
                Comment = model.Comment,
                BidDate = DateTime.Now,
                BidderId = loggedUserId,
                OfferId = id
            };
            this.Data.Bids.Add(newBid);
            this.Data.SaveChanges();
            return this.Ok(new { Id = newBid.Id, Bidder = user.UserName, message = "Bid created." });
        }
    }
}