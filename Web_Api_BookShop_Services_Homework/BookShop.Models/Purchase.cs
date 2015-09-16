using System;
using BookShop.Models;

namespace BookShopServices.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        public double Price { get; set; }

        public DateTime DateOfPurchase { get; set; }

        public bool IsRecalled { get; set; }

        public virtual Book Book { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}