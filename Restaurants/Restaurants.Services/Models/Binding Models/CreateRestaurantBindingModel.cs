namespace Restaurants.Services.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateRestaurantBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int TownId { get; set; }
    }
}