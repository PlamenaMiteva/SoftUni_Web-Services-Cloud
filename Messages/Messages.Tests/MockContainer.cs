namespace Messages.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Messages.Data;
    using Messages.Data.Models;
    using Messages.Data.Repositories;
    using Moq;

    public class MockContainer
    {
        public Mock<IMessagesData> MockData { get; set; }

        public Mock<IRepository<Channel>> ChannelsMock { get; set; }

        public Mock<IRepository<ChannelMessage>> ChannelMessagesMock { get; set; }

        public Mock<IRepository<UserMessage>> UserMessagesMock { get; set; }

        public void SetupMocks()
        {
            var fakeChannels = new[]
            {
                new Channel()
                {
                    Id = 1,
                    Name = "BNT"
                },
                new Channel()
                {
                    Id = 2,
                    Name = "bTV"
                }
            };

            this.PrepareFakeChannels(fakeChannels);

           this.PrepareFakeData();
        }

        private void PrepareFakeData()
        {
            this.MockData = new Mock<IMessagesData>();

            this.MockData.Setup(d => d.Channels)
                .Returns(this.ChannelsMock.Object);
        }

        private void PrepareFakeChannels(IEnumerable<Channel> channels)
        {
            this.ChannelsMock = new Mock<IRepository<Channel>>();
            this.ChannelsMock.Setup(r => r.All())
                .Returns(channels.AsQueryable());
        }

        
    }
}
