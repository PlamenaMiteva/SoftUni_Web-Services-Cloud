using System;
using System.Linq;
using System.Linq.Expressions;
using BidSystem.Data.Models;

namespace BidSystem.RestServices.Models
{
    public class BidsViewModel
    {
        public int Id { get; set; }

        public int OfferId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Bidder { get; set; }

        public double OfferdPrice { get; set; }

        public string Comment { get; set; }

        public static Expression<Func<Bid, BidsViewModel>> Create
        {
            get
            {
                return b => new BidsViewModel
                {
                    Id = b.Id,
                    OfferId = b.OfferId,
                    DateCreated = b.BidDate,
                    Bidder = b.Bidder.UserName,
                    OfferdPrice = b.BidPrice,
                    Comment = b.Comment
                };
            }
        }
    }
}