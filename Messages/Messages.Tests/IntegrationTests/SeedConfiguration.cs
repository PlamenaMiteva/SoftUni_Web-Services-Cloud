using System;
using System.Collections.Generic;
using System.Linq;
using Messages.Data;
using Messages.Data.Models;
using Messages.RestServices;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Messages.Tests.IntegrationTests
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
            var context = new MessagesDbContext();
            if (!context.Users.Any(u => u.UserName == TestUserUsername))
            {
                SeedUsers(context);
            }

            if (!context.Channels.Any())
            {
                SeedChannels(context);
            }
            }

        private void SeedChannels(MessagesDbContext context)
        {
            var channel = new Channel()
            {
                Name = "Channel1"
            };
            context.Channels.Add(channel);
            context.SaveChanges();
            //var message = new ChannelMessage()
            //{
            //    Text = "New message",
            //    DateSent = DateTime.Now,
            //    Sender = context.Users.FirstOrDefault(),
            //    Channel = channel
            //};
            //context.ChannelMessages.Add(message);
            //context.SaveChanges();
            var channel2 = new Channel()
                {
                    Name = "Channel2",
                    ChannelMessages = new HashSet<ChannelMessage>()
                };
            context.Channels.Add(channel2);
            context.SaveChanges();
        }

        

        private void SeedUsers(MessagesDbContext context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new ApplicationUserManager(userStore);

            var user = new User()
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
