using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restauranteur.Models;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services;

namespace Tests
{
    public class SeedConfiguration
    {
        public SeedConfiguration(string userUsername, string userPassword)
        {
            this.TestUserUsername = userUsername;
            this.TestUserPassword = userPassword;
        }

        public string TestUserUsername { get; set; }

        public string TestUserPassword { get; set; }

        public void Seed()
        {
            var context = new RestaurantsContext();
            if (!context.Users.Any(u => u.UserName == TestUserUsername))
            {
                SeedUsers(context);
            }

            if (!context.Meals.Any())
            {
                var mealTypes = SeedMealTypes(context);

                SeedMeals(context, mealTypes);
            }
        }

        private void SeedMeals(RestaurantsContext context, List<MealType> mealTypes)
        {
            var meals = new List<Meal>()
            {
                new Meal()
                {
                    Name = "French salad",
                    Price = 5.40m,
                    Restaurant = new Restaurant()
                    {
                        Name = "Pri baba",
                        Owner = context.Users.FirstOrDefault(u => u.UserName == TestUserUsername),
                        Town = new Town() {Name = "Bracigovo"}
                    },
                    Type = mealTypes.First()
                }
            };

            foreach (var meal in meals)
            {
                context.Meals.Add(meal);
            }

            context.SaveChanges();
        }

        private List<MealType> SeedMealTypes(RestaurantsContext context)
        {
            var mealTypes = new List<MealType>()
            {
                new MealType() {Name = "Salad", Order = 10},
                new MealType() {Name = "Soup", Order = 20}
            };

            foreach (var mealType in mealTypes)
            {
                context.MealTypes.Add(mealType);
            }

            context.SaveChanges();
            return mealTypes;
        }

        private void SeedUsers(RestaurantsContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(userStore);

            var user = new ApplicationUser()
            {
                UserName = TestUserUsername,
                Email = string.Format("{0}@gmail.com", TestUserUsername)
            };

            var userResult = userManager.CreateAsync(user, TestUserPassword).Result;
            if (!userResult.Succeeded)
            {
                Assert.Fail(string.Join("\n", userResult.Errors));
            }
        }
    }
}
