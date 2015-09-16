using BookShop.Models;

namespace BookShopServices.Models.ViewModels
{
    public class BookViewModel
    {
        public string Title { get; set; }

        public BookViewModel(Book book)
        {
            Title = book.Title;
        }
    }
}