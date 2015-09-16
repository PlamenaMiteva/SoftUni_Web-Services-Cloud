using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BookShop.Data;
using BookShop.Models;
using BookShopServices.Models.ViewModels;

namespace BookShopServices.Controllers
{
    [RoutePrefix("api/authors")]
    public class AuthorsController : BaseApiController
    {
        //GET/api/authors/{id}
        [Route("{id}")]
        public IHttpActionResult GetAuthor(int id)
        {  
            Author author = this.Data.Authors.Find(id);
            if (author==null)
            {
                return NotFound();
            }
            return Ok(new AuthorViewModel(author));
        }

        //POST/api/authors
        public void PostAuthor(string firstName, string lastName)
        {
            Author newAuthor = new Author()
            {
                FirstName = firstName,
                LastName = lastName
            };
            this.Data.Authors.Add(newAuthor);
            this.Data.SaveChanges();
        }

        //GET/api/authors/{id}/books
        [Route("{authorId}/books")]
        public IHttpActionResult GetAuthorBooks(int authorId)
        {
            var books = this.Data.Books.Where(b => b.Author.Id == authorId);
            List<AuthorBooksViewModel> authorBooks= new List<AuthorBooksViewModel>();
            foreach (var book in books)
            {
                authorBooks.Add(new AuthorBooksViewModel(book));
            }
            if (!books.Any())
            {
                return NotFound();
            }
            return Ok(authorBooks);
        }
    }
}
