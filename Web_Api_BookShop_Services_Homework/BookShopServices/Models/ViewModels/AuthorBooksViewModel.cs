using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShopServices.Models.ViewModels
{
    public class AuthorBooksViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Copies { get; set; }

        public EditionType Edition { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? AgeRestriction { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; }

        public AuthorBooksViewModel(Book book)
        {
            Title = book.Title;
            Description = book.Description;
            Price = book.Price;
            Copies = book.Copies;
            ReleaseDate = book.ReleaseDate;
            Edition = book.Edition;
            AgeRestriction = book.AgeRestriction;
            Categories = new List<CategoryViewModel>();
            foreach (var category in book.Categories)
            {
                Categories.Add(new CategoryViewModel(category));
            }
        }
    }
}