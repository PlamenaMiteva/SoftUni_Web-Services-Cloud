using System;
using System.ComponentModel.DataAnnotations;

namespace BidSystem.RestServices.Models
{
    public class CreateNewOfferBindingModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public double InitialPrice { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}