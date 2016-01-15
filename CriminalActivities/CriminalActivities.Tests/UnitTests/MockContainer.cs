namespace CriminalActivities.Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using CriminalActivities.Data.Repositories;
    using Models;
    using Moq;

    public class MockContainer
    {
        public Mock<ICriminalActivitiesData> MockData { get; set; }

        public Mock<IRepository<Criminal>> CriminalsMock { get; set; }

        public Mock<IRepository<Cartel>> CartelsMock { get; set; }

        public void SetupMocks()
        {
            var fakeCartels = new[]
            {
                new Cartel()
                {
                    Id = 1,
                    Name = "Cosa Nostra"
                },
                new Cartel()
                {
                    Id = 2,
                    Name = "La Drangada"
                }
            };

            var fakeCriminals = new[]
            {
                new Criminal() 
                {
                    Id = 1,
                    FullName = "Gaetano Mosca",
                    Alias = "Mosca",
                    CartelId = 1
                },
                new Criminal() 
                {
                    Id = 2,
                    FullName = "El Mafioso",
                    Alias = "Mafioso",
                    CartelId = 2
                }
            };
            this.PrepareFakeCartels(fakeCartels);

            this.PrepareFakeCriminals(fakeCriminals);

            this.PrepareFakeData();
        }

        private void PrepareFakeData()
        {
            this.MockData = new Mock<ICriminalActivitiesData>();

            this.MockData.Setup(d => d.Criminals)
                .Returns(this.CriminalsMock.Object);

            this.MockData.Setup(d => d.Cartels)
                .Returns(this.CartelsMock.Object);
        }

        private void PrepareFakeCriminals(IEnumerable<Criminal> criminals)
        {
            this.CriminalsMock = new Mock<IRepository<Criminal>>();
            this.CriminalsMock.Setup(r => r.All())
                .Returns(criminals.AsQueryable());
        }

        private void PrepareFakeCartels(IEnumerable<Cartel> cartels)
        {
            this.CartelsMock = new Mock<IRepository<Cartel>>();
            this.CartelsMock.Setup(r => r.All())
                .Returns(cartels.AsQueryable());
        }
    }
}
