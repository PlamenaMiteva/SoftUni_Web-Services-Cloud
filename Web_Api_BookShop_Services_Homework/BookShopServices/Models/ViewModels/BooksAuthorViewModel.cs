using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShopServices.Models.ViewModels
{
    public class BooksAuthorViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public BooksAuthorViewModel(Author author)
        {
            Id = author.Id;
            FirstName = author.FirstName;
            LastName = author.LastName;
        }
    }
}