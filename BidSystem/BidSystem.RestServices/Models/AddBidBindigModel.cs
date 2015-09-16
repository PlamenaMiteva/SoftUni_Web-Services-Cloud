using System.ComponentModel.DataAnnotations;

namespace BidSystem.RestServices.Models
{
    public class AddBidBindingModel
    {
        [Required]
        public double BidPrice { get; set; }

        public string Comment { get; set; }
    }
}