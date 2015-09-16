using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EntityFramework.Extensions;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineShop.Models;
using OnlineShop.Services;
using Owin;
using OnlineShop.Data;

namespace OnlineShop.IntegrationTests
{
    [TestClass]
    public class AdsIntegrationTests
    {
        private static TestServer httpTestServer;
        private static HttpClient httpClient;
        private string accessToken;


        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            httpTestServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                var startup = new Startup();

                startup.Configuration(appBuilder);
                appBuilder.UseWebApi(config);
            });
            httpClient = httpTestServer.HttpClient;
            SeedDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanUp()
        {
            if (httpTestServer != null)
            {
                httpTestServer.Dispose();
            }
            CleanDatabase();
        }

        private static void SeedDatabase()
        {
            var context = new OnlineShopContext();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(userStore);
            var user = new ApplicationUser()
            {
                UserName = "Test",
                Email = "test@abv.bg"
            };

            var result = userManager.CreateAsync(user, "Test123%").Result;
            if (!result.Succeeded)
            {
                Assert.Fail(string.Join(Environment.NewLine, result.Errors));
            }
            context.Categories.Add(new Category() { Name = "Auto" });
            context.Categories.Add(new Category() { Name = "Education" });
            context.Categories.Add(new Category() { Name = "home and Family" });
            context.AdTypes.Add(new AdType() { Name = "Economy", PricePerDay = 10 });
            context.AdTypes.Add(new AdType() { Name = "Normal", PricePerDay = 20 });
            context.AdTypes.Add(new AdType() { Name = "Lux", PricePerDay = 40 });
            context.SaveChanges();
        }

        private static void CleanDatabase()
        {
            var context = new OnlineShopContext();

            context.Ads.Delete();
            context.AdTypes.Delete();
            context.Users.Delete();
            context.Categories.Delete();
        }

        private HttpResponseMessage Login()
        {
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", "Test"), 
                new KeyValuePair<string, string>("Password", "Test123%"), 
                new KeyValuePair<string, string>("grant_type", "password")
            });
            var response = httpClient.PostAsync("/Token", loginData).Result;
            return response;
        }

        public string AccessToken
        {
            get
            {
                if (this.accessToken == null)
                {
                    var loginResponse = this.Login();

                    if (!loginResponse.IsSuccessStatusCode)
                    {
                        Assert.Fail("Unable to Login: " + loginResponse.ReasonPhrase);
                    }

                    var loginData = loginResponse.Content.ReadAsAsync<LoginDto>().Result;

                    this.accessToken = loginData.Access_Token;
                }

                return this.accessToken;
            }
        }

        private HttpResponseMessage PostNewAd(FormUrlEncodedContent data)
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AccessToken);
            }

            var response = httpClient.PostAsync("/api/ads", data).Result;

            return response;
        }

        [TestMethod]
        public void Login_Should_Return_200OK_And_Access_Token()
        {
            var loginResponse = this.Login();
            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginData = loginResponse.Content.ReadAsAsync<LoginDto>().Result;
            Assert.IsNotNull(loginData.Access_Token);
        }

        [TestMethod]
        public void Posting_Ad_With_Invalid_AdType_Should_Return_BadRequest()
        {
            var context = new OnlineShopContext();
            var category = context.Categories.First();

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", "OpelAstra"), 
                new KeyValuePair<string, string>("description", "A brand new car"), 
                new KeyValuePair<string, string>("price", "2009.67"), 
                new KeyValuePair<string, string>("typeId", "-1"), 
                new KeyValuePair<string, string>("categories[0]", category.Id.ToString())
            });
            var response = this.PostNewAd(data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
