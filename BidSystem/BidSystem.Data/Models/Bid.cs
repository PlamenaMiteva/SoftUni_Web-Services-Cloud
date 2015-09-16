using System;
using System.ComponentModel.DataAnnotations;

namespace BidSystem.Data.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double BidPrice { get; set; }

        [Required]
        public DateTime BidDate { get; set; }

        public string Comment { get; set; }

        [Required]
        public string BidderId { get; set; }

        public virtual User Bidder { get; set; }

        [Required]
        public int OfferId { get; set; }

        public virtual Offer Offer { get; set; }
    }
}
