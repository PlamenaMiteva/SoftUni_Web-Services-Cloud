using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShopServices.Models.ViewModels
{
    public class UserViewModel
    {
        public string Username { get; set; }

        public ICollection<PurchaseViewModel> Purchases { get; set; }

        public UserViewModel(ApplicationUser user)
        {
            Username = user.UserName;
            Purchases = new List<PurchaseViewModel>();
            foreach (var purchase in user.Purchases.OrderBy(p=>p.DateOfPurchase))
            {
                Purchases.Add(new PurchaseViewModel(purchase));
            }
            
        }
    }
}