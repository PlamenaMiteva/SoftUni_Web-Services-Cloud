using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.Security;
using BookShop.Data;
using BookShop.Models;
using BookShopServices.Models;
using BookShopServices.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookShopServices.Controllers
{
    [RoutePrefix("api/books")]
    public class BooksController : BaseApiController
    {
        
        
        //GET/api/books/{id}
        [Route("{id}")]
        public IHttpActionResult GetBook(int bookId)
        {
            Book book = this.Data.Books.Find(bookId);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(new BookByIdViewModel(book));
        }

        ///GET/api/books?search={word}  -  without OData
        //public IHttpActionResult GetBookByWord(string search)
        //{
        //    var books = this.Data.Books.Where(b => b.Title.ToLower().Contains(search.ToLower())).OrderBy(b => b.Title).ToList();
        //    if (!books.Any())
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        List<SearchedBookViewModel> searchedBooks = new List<SearchedBookViewModel>();
        //        for (int i = 0; i < 10; i++)
        //        {
        //            searchedBooks.Add(new SearchedBookViewModel(books[i]));
        //        }
        //        return Ok(searchedBooks);
        //    }
        //}

        ///GET/api/books?search={word}  -  using OData
        [EnableQueryAttribute(PageSize = 10, AllowedOrderByProperties = "Title")]
        public IQueryable<SearchedBookViewModel> GetBookByWord(string search)
        {
            var books = this.Data.Books.Where(b => b.Title.ToLower().Contains(search.ToLower()));
            List<SearchedBookViewModel> searchedBooks = new List<SearchedBookViewModel>();
            foreach (var book in books)
            {
                searchedBooks.Add(new SearchedBookViewModel(book));
            }
            return searchedBooks.AsQueryable();
        }

        //PUT/api/books/{id}
        [Route("{bookId}")]
        public void PutBook(int bookId, string title, string description, double price, int copies, 
            DateTime releasedData, EditionType edition, int ageRestriction, int authorId)
        {
            var query = this.Data.Books.Find(bookId);
            var author = this.Data.Authors.Find(authorId);
            query.Title = title;
            query.Description = description;
            query.Edition = edition;
            query.ReleaseDate = releasedData;
            query.Price = price;
            query.Copies = copies;
            query.Author = author;
            query.AgeRestriction = ageRestriction;
            this.Data.SaveChanges();
        }

        // DELETE api/books/5
        [Route("{id}")]
        public void DeleteBook(int bookId)
        {
            var query = this.Data.Books.Find(bookId);
            this.Data.Books.Remove(query);
            this.Data.SaveChanges();
        }

        //POST/api/books
        public void PostBook(string title, string description, double price, int copies,
            DateTime releasedData, EditionType edition, int AgeRestriction, string categoryNames)
        {
            Book newBook = new Book()
            {
            Title = title,
            Description = description,
            Edition = edition,
            ReleaseDate = releasedData,
            Price = price,
            Copies = copies,
            AgeRestriction = AgeRestriction
            };
            string[] bookCategories = categoryNames.Split(' ').ToArray();
            foreach (var category in bookCategories)
            {
                var currentCategory = this.Data.Categories.First(c => c.Name == category);
                newBook.Categories.Add(currentCategory);

            }
            this.Data.Books.Add(newBook);
            this.Data.SaveChanges();
        }
        //PUT/api/books/buy/{id}
        [Route("buy/{id}"), Authorize]
        public void PutPurchase(int id)
        {
            string userId = User.Identity.GetUserId();
            var user=this.Data.Users.Find(userId);
            var book = this.Data.Books.Find(id);
            book.Copies -= 1;
            Purchase newPurchase = new Purchase()
            {
                Book = book,
                Price = book.Price,
                IsRecalled = false,
                DateOfPurchase = DateTime.Now,
                User=user
            };
            this.Data.Purchases.Add(newPurchase);
            this.Data.SaveChanges();
        }
        //PUT/api/books/recall/{id}
        [Route("recall/{id}")]
        public void PutRecalledBook(int id)
        {
            var book = this.Data.Books.Find(id);
            book.Copies += 1;
            var bookpurchases = this.Data.Purchases.Where(p => p.Book.Id == id);
            if (bookpurchases.Any())
            {
                foreach (var purchase in bookpurchases)
                {
                    if ((DateTime.Now - purchase.DateOfPurchase).TotalDays > 30)
                    {
                        purchase.IsRecalled = true;
                    }
                }
            }
            this.Data.SaveChanges();
        }
    }
}
