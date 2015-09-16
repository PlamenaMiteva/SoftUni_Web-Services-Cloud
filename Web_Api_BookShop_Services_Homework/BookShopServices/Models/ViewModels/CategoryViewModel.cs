using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookShop.Models;

namespace BookShopServices.Models.ViewModels
{
    public class CategoryViewModel
    {
        public string Name { get; set; }
        public CategoryViewModel(Category category)
        {
            Name = category.Name;
        }
    }
}