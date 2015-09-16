using System.Web.Http;
using BookShop.Data;
using Microsoft.Ajax.Utilities;

namespace BookShopServices.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
            :this(new BookShopContext())
        {
        }

        public BaseApiController(BookShopContext data)
        {
            this.Data = data;
        }

        public BookShopContext Data { get; set; }
    }
}
