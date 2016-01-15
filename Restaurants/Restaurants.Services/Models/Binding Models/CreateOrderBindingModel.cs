namespace Restaurants.Services.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderBindingModel
    {
        [Required]
        public uint Quantity { get; set; }
    }
}