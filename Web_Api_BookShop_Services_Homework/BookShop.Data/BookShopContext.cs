using BookShop.Data.Migrations;
using BookShopServices.Models;

namespace BookShop.Data
{
    using System.Data.Entity;
    using BookShop.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
   
    public class BookShopContext : IdentityDbContext<ApplicationUser>
    {
        public BookShopContext()
            : base("name=BookShopContext")
        {
            var migrationStrategy = new MigrateDatabaseToLatestVersion<BookShopContext, Configuration>();
            Database.SetInitializer(migrationStrategy);
        }

        public static BookShopContext Create()
        {
            return new BookShopContext();
        }
        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Author> Authors { get; set; }

        public virtual DbSet<Purchase> Purchases{ get; set; }
        

        }
}