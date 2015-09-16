using System.Collections.Generic;

namespace BugTracker.Data.Models
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Bug> ownBugs;
        private ICollection<Comment> ownComments;

        public User()
        {
            this.ownBugs = new HashSet<Bug>();
            this.ownComments = new HashSet<Comment>();
        }
        public virtual ICollection<Bug> OwnBugs
        {
            get { return this.ownBugs; }
            set { this.ownBugs = value; }
        }

        public virtual ICollection<Comment> OwnComments
        {
            get { return this.ownComments; }
            set { this.ownComments = value; }
        }

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
