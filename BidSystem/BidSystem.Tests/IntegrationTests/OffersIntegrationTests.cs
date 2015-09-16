using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BidSystem.Data;
using BidSystem.Data.Models;
using BidSystem.RestServices;
using BidSystem.RestServices.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Owin;

namespace BidSystem.Tests.IntegrationTests
{
    [TestClass]
    public class OffersIntegrationTests
    {
        private static TestServer server;
        private static HttpClient client;

        [AssemblyInitialize]
        public static void InitializeTests(TestContext context)
        {
            server = TestServer.Create(AppBuilder =>
            {
                var httpConfig = new HttpConfiguration();
                WebApiConfig.Register(httpConfig);
                var webApiStartup = new Startup();
                webApiStartup.Configuration(AppBuilder);
                AppBuilder.UseWebApi(httpConfig);
            });
            client = server.HttpClient;
            Seed();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        private static void Seed()
        {
            var context = new BidSystemDbContext();
            var userStore = new UserStore<User>(context);
            var userManager = new ApplicationUserManager(userStore);
            var user = new User()
            {
                UserName = "Ivano",
                Email = "test@abv.bg"
            };
            var result = userManager.CreateAsync(user, "TestUserPassword").Result;
            if (!result.Succeeded)
            {
                Assert.Fail(string.Join(Environment.NewLine, result.Errors));
            }
            var offer = new Offer()
            {
                Title = "First offer",
                Description = "first offer description",
                PublishDate = DateTime.Now,
                InitialPrice = 34.55,
                ExpirationhDate = new DateTime(2015, 12, 12, 2, 40, 34),
                Seller = user
            };
            context.Offers.Add(offer);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetOfferDetails_ShouldReturn200OK_ExistingOffer()
        {
            var dbContext = new BidSystemDbContext();
            var existingOffer = dbContext.Offers.FirstOrDefault();
            if (existingOffer == null)
            {
                Assert.Fail("Cannot perform test - no offers in DB.");
            }
            var endpoint = string.Format("api/offers/details/{0}", existingOffer.Id);
            var response = client.GetAsync(endpoint).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<List<OfferViewModel>>(result);
            Assert.AreEqual(existingOffer.Title, data[0].Title);
        }

        [TestMethod]
        public void GetOfferDetails_ShouldReturnNotFound_NonExistingOffer()
        {
            var dbContext = new BidSystemDbContext();
            var endpoint = string.Format("api/offers/details/100");
            var response = client.GetAsync(endpoint).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
