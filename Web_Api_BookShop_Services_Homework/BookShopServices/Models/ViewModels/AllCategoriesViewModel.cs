using BookShop.Models;

namespace BookShopServices.Models.ViewModels
{
    public class AllCategoriesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AllCategoriesViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }
    }
}