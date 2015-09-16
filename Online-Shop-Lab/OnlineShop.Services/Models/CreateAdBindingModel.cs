using System.Collections.Generic;

namespace OnlineShop.Services.Models
{
    public class CreateAdBindingModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }

        public decimal Price { get; set; }
        
        public IEnumerable<int> Categories { get; set; }
    }
}