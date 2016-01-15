namespace Restaurants.Services.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;
  
    public class CreateMealBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public int TypeId { get; set; }

    }
}