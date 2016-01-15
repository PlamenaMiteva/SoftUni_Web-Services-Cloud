namespace Restaurants.Services.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;

    public class SearchRestaurantsBindingModel
    {
        [Required]
        public int TownId { get; set; }
    }
}