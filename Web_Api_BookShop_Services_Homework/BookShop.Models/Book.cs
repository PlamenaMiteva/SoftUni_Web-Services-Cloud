using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookShopServices.Models;

namespace BookShop.Models
{
    public class Book
    {

        private ICollection<Category> categories;
        private ICollection<Purchase> purchases;
        public Book()
        {
            this.categories = new HashSet<Category>();
            this.purchases = new HashSet<Purchase>();
        }
        public int Id { get; set; }

        [Required, MaxLength(50), MinLength(1)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Copies { get; set; }

        public EditionType Edition { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? AgeRestriction { get; set; }

        public virtual Author Author { get; set; }

        public virtual ICollection<Category> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }
        public virtual ICollection<Purchase> Purchases
        {
            get { return this.purchases; }
            set { this.purchases = value; }
        }
        }
}
