using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BidSystem.Data.Models;

namespace BidSystem.RestServices.Models
{
    public class OffersViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Seller { get; set; }

        public DateTime DatePublished { get; set; }

        public double InitialPrice { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public bool IsExpired { get; set; }

        public int BidsCount { get; set; }

        public string BidWinner { get; set; }

        public static Expression<Func<Offer, OffersViewModel>> Create
        {
            get
            {
                return o => new OffersViewModel
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Seller = o.Seller.UserName,
                    DatePublished = o.PublishDate,
                    InitialPrice = o.InitialPrice,
                    ExpirationDateTime = o.ExpirationhDate,
                    IsExpired = o.ExpirationhDate <= DateTime.Now,
                    BidsCount = o.Bids.Count(),
                    BidWinner = (o.ExpirationhDate <= DateTime.Now && o.Bids.Any())
                        ? o.Bids.OrderByDescending(b => b.BidPrice).FirstOrDefault().Bidder.UserName
                        : null
                };
            }
        }
    }
}