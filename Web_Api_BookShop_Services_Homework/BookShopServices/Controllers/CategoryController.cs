using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using BookShop.Data;
using BookShop.Models;
using BookShopServices.Models.ViewModels;

namespace BookShopServices.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoryController : BaseApiController
    {
        
        //GET/api/categories
        public IHttpActionResult GetAllCategories()
        {
            var categories = this.Data.Categories.ToList();
            List<AllCategoriesViewModel> allCategories = new List<AllCategoriesViewModel>();
            foreach (var category in categories)
            {
                allCategories.Add(new AllCategoriesViewModel(category));
            }
            if (!allCategories.Any())
            {
                return NotFound();
            }
            return Ok(allCategories);
        }


        //GET/api/categories -  using OData
        //[EnableQuery]
        //public IQueryable<AllCategoriesViewModel> GetAllCategories()
        //{
        //    return this.Data.Categories.
        //        Select(c => new AllCategoriesViewModel(new Category()
        //        {
        //            Id = c.Id,
        //            Name = c.Name

        //        })).AsQueryable();
        //}

        //GET/api/categories/{id}
        [Route("{id}")]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = this.Data.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(new AllCategoriesViewModel(category));
        }

        //PUT/api/categories/{id}
        [Route("{id}")]
        public void PutCategory(int id, string name)
        {
            var categories = this.Data.Categories.Where(c=>c.Name==name).ToList();
            if (categories.Count==0)
            {
                var query = this.Data.Categories.Find(id);
            query.Name = name;
            this.Data.SaveChanges();
            }
        }

        // DELETE api/categories/5
        [Route("{id}")]
        public void DeleteCategory(int id)
        {
            var query = this.Data.Categories.Find(id);
            this.Data.Categories.Remove(query);
            this.Data.SaveChanges();
        }

        //POST/api/categories
       public void PostCategory(string name)
        {
            var categories = this.Data.Categories.Where(c=>c.Name==name).ToList();
            if (categories.Count == 0)
            {
                Category newCategory = new Category()
                {
                    Name = name
                };
                this.Data.Categories.Add(newCategory);
                this.Data.SaveChanges();
            }
        }
    }
}
