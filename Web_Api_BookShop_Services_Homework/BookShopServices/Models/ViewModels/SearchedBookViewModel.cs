using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShopServices.Models.ViewModels
{
    public class SearchedBookViewModel
    {
        public int Id { get; set; }
            
        public string Title { get; set; }

        public SearchedBookViewModel(Book book)
        {
            Id = book.Id;
            Title = book.Title;
        }
    }
}