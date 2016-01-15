namespace CriminalActivities.Tests.IntegrationTests
{
    using System.Linq;
    using CriminalActivities.App;
    using CriminalActivities.Data;
    using CriminalActivities.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var context = new CriminalContext();
            if (!context.Users.Any(u => u.UserName == TestUserUsername))
            {
                SeedUsers(context);
            }
        }


        private void SeedUsers(CriminalContext context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new ApplicationUserManager(userStore);

            var user = new User()
            {
                UserName = TestUserUsername,
                Email = string.Format("{0}@gmail.com", TestUserUsername)
            };

            var userResult = userManager
                .CreateAsync(user, TestUserPassword).Result;
            if (!userResult.Succeeded)
            {
                Assert.Fail(string.Join("\n", userResult.Errors));
            }
        }
    }
}
