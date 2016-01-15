namespace Restaurants.Services.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;

    public class RateBindingModel
    {
        [Range(0, 10)]
        public int Stars { get; set; }
    }
}