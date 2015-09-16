using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BidSystem.Data.Models;

namespace BidSystem.RestServices.Models
{
    public class OfferViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Seller { get; set; }

        public DateTime DatePublished { get; set; }

        public double InitialPrice { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public bool IsExpired { get; set; }

        public string BidWinner { get; set; }

        public IEnumerable<BidsViewModel> Bids { get; set; }

        public static Expression<Func<Offer, OfferViewModel>> Create
        {
            get
            {
                return o => new OfferViewModel
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Seller = o.Seller.UserName,
                    DatePublished = o.PublishDate,
                    InitialPrice = o.InitialPrice,
                    ExpirationDateTime = o.ExpirationhDate,
                    IsExpired = o.ExpirationhDate <= DateTime.Now,
                    BidWinner = (o.ExpirationhDate <= DateTime.Now == true && o.Bids.Count() >= 0) ? o.Bids.OrderByDescending(b => b.BidPrice).FirstOrDefault().Bidder.UserName : null,
                    Bids = o.Bids.OrderByDescending(b => b.BidDate).
                    Select(b => new BidsViewModel()
                    {
                        Id = b.Id,
                        OfferId = b.OfferId,
                        DateCreated = b.BidDate,
                        Bidder = b.Bidder.UserName,
                        OfferdPrice = b.BidPrice,
                        Comment = b.Comment
                    }),

                };
            }
        }
    }
}