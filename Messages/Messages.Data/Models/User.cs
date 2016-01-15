namespace Messages.Data.Models
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<ChannelMessage> channelMessages;
        //private ICollection<UserMessage> userSentMessages;
        //private ICollection<UserMessage> userRecievedMessages;

        public User()
        {
            this.channelMessages = new HashSet<ChannelMessage>();
            //this.userSentMessages = new HashSet<UserMessage>();
            //this.userRecievedMessages = new HashSet<UserMessage>();
        }

        public virtual ICollection<ChannelMessage> ChannelMessages
        {
            get { return this.channelMessages; }
            set { this.channelMessages = value; }
        }

        //public virtual ICollection<UserMessage> UserSentMessages
        //{
        //    get { return this.userSentMessages; }
        //    set { this.userSentMessages = value; }

        //}

        //public virtual ICollection<UserMessage> UserRecievedMessages
        //{
        //    get { return this.userRecievedMessages; }
        //    set { this.userRecievedMessages = value; }
        //}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
