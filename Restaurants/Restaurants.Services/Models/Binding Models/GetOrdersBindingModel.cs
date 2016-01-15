namespace Restaurants.Services.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;

    public class GetOrdersBindingModel
    {
        public int StartPage { get; set; }

        [Range(2, 10)]
        public int Limit { get; set; }

        public int? MealId { get; set; }   
    }
}