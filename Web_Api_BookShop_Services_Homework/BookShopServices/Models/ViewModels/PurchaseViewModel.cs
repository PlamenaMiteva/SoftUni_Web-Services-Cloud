using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShopServices.Models.ViewModels
{
    public class PurchaseViewModel
    {
        public double Price { get; set; }

        public DateTime DateOfPurchase { get; set; }

        public bool IsRecalled { get; set; }

        public BookViewModel Book { get; set; }

        public PurchaseViewModel(Purchase purchase)
        {
            Price = purchase.Price;
            DateOfPurchase = purchase.DateOfPurchase;
            IsRecalled = purchase.IsRecalled;
            Book = new BookViewModel(purchase.Book);
        }
    }
}