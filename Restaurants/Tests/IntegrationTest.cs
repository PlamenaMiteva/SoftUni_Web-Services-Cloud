using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;
using Restaurants.Data;
using Restaurants.Services;
using Restaurants.Services.Models.ViewModels;

namespace Tests
{
    [TestClass]
    public class IntegrationTest
    {
        private const string TestUserUsername = "admin";
        private const string TestUserPassword = "Admin123";

        private static TestServer server;
        private static HttpClient client;

        private string accessToken;

        private string AccessToken
        {
            get
            {
                if (this.accessToken == null)
                {
                    var loginResponse = this.Login();
                    if (!loginResponse.IsSuccessStatusCode)
                    {
                        Assert.Fail("Unable to login: " + loginResponse.ReasonPhrase);
                    }

                    var loginData = loginResponse.Content
                        .ReadAsAsync<LoginDTO>().Result;

                    this.accessToken = loginData.Access_Token;
                }

                return this.accessToken;
            }
        }

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
            var seedConfig = new SeedConfiguration(TestUserUsername, TestUserPassword);
            seedConfig.Seed();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }


        [TestMethod]
        public void EditExistingOwnersMeal_ShouldReturn200OK_And_ChangedMeal()
        {
            var dbContext = new RestaurantsContext();
            var meal = dbContext.Meals.First(m => m.Restaurant.Owner.UserName == TestUserUsername);
            var type = dbContext.MealTypes.First(t => t.Id != meal.TypeId);
            this.SetAuthorizationHeaders(true);
            var newType = type.Name;
            var response = this.SendEditMealRequest(meal.Id, "Italian salad", 12.43m, meal.TypeId);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = response.Content.ReadAsAsync<MealViewModel>().Result;
            Assert.AreEqual("Italian salad", result.Name);
            Assert.AreEqual(12.43m, result.Price);
            Assert.AreEqual(newType, result.Type);
        }

        [TestMethod]
        public void EditExistingMeal_AnotherOwner_ShouldReturn401Unauthorized()
        {
            var dbContext = new RestaurantsContext();
            var meal = dbContext.Meals.First(m => m.Restaurant.Owner.UserName == TestUserUsername);
            var type = dbContext.MealTypes.First(t => t.Id != meal.TypeId);
            this.SetAuthorizationHeaders(false);
            var response = this.SendEditMealRequest(meal.Id, "Italian salad", 12.43m, meal.TypeId);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void EditNonExistingOwnersMeal_ShouldReturn404NotFound()
        {
            var dbContext = new RestaurantsContext();
            var type = dbContext.MealTypes.First();
            this.SetAuthorizationHeaders(true);
            var response = this.SendEditMealRequest(2000, "Italian salad", 12.43m, type.Id);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void EditExistingOwnersMealWithInvalidData_ShouldReturn400BadRequest()
        {
            var dbContext = new RestaurantsContext();
            var meal = dbContext.Meals.First(m => m.Restaurant.Owner.UserName == TestUserUsername);
            var type = dbContext.MealTypes.First(t => t.Id != meal.TypeId);
            this.SetAuthorizationHeaders(true);
            var response = this.SendEditMealRequest(meal.Id, "", 12.43m, meal.TypeId);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private HttpResponseMessage SendEditMealRequest(int mealId, string name, decimal price, int typeId)
        {
            var model = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("price", price.ToString()),
                new KeyValuePair<string, string>("typeId", typeId.ToString())
            });

            return client.PutAsync("api/meals/" + mealId, model).Result;
        }

        private HttpResponseMessage Login()
        {
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", TestUserUsername),
                new KeyValuePair<string, string>("password", TestUserPassword),
                new KeyValuePair<string, string>("grant_type", "password")
            });

            var response = client.PostAsync("api/account/login", loginData).Result;

            return response;
        }

        private void SetAuthorizationHeaders(bool isLogged)
        {
            client.DefaultRequestHeaders.Remove("Authorization");
            if (isLogged)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AccessToken);
            }
        }
    }
}
