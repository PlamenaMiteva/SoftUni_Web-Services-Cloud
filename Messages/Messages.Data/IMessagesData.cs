using Messages.Data.Models;
using Messages.Data.Repositories;

namespace Messages.Data
{
    public interface IMessagesData
    {
        IRepository<Channel> Channels { get; }

        IRepository<User> Users { get; }

        IRepository<ChannelMessage> ChannelMessages { get; }

        IRepository<UserMessage> UserMessages { get; }

        int SaveChanges();
    }
}
