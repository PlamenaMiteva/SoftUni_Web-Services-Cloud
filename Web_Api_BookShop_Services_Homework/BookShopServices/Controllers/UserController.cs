using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookShop.Data;
using BookShop.Models;
using BookShopServices.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace BookShopServices.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        

        //GET/api/user/{username}/purchases
        [Route("{username}/purchases")]
        public IHttpActionResult GetUserPurchases(string username)
        {
            var user = this.Data.Users.First(u => u.UserName == username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new UserViewModel(user));
        }
    }
}
