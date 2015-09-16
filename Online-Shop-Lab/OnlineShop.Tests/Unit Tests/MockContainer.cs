using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Moq;
using OnlineShop.Data.Repositories;
using OnlineShop.Models;

namespace OnlineShop.Tests.Unit_Tests
{
    public class MockContainer
    {
        public Mock<IRepository<Ad>> AdRepositoryMock { get; set; }

        public Mock<IRepository<AdType>> AdTypeRepositoryMock { get; set; }

        public Mock<IRepository<Category>> CategoryRepositoryMock { get; set; }

        public Mock<IRepository<ApplicationUser>> UserRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetUpFakeCategories();

            this.SetUpFakeUsers();

            this.SetUpFakeAds();

            this.SetUpFakeAdTypes();


        }

        private void SetUpFakeUsers()
        {
            var users = new List<ApplicationUser>()
            {
               new ApplicationUser()
                  {
                  Id = "1",
                  UserName = "Plamena"
                  }
            };
            this.UserRepositoryMock = new Mock<IRepository<ApplicationUser>>();

            this.UserRepositoryMock.Setup(r => r.All()).Returns(users.AsQueryable());

            this.UserRepositoryMock.Setup(r => r.Find(It.IsAny<string>()))
                .Returns((string id) => users.FirstOrDefault(user => user.Id == id));
        }
        private void SetUpFakeCategories()
        {
            var categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Autos"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Motos"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Vehicles"
                },
            };
            this.CategoryRepositoryMock = new Mock<IRepository<Category>>();

            this.CategoryRepositoryMock.Setup(c => c.All()).Returns(categories.AsQueryable());

            this.CategoryRepositoryMock.Setup(c => c.Find(It.IsAny<int>()))
                .Returns((int id) => categories.FirstOrDefault(c => c.Id == id));
        }
        private void SetUpFakeAds()
        {
            var adTypes = new List<AdType>()
            {
                new AdType()
                {
                    Name = "Normal", Index = 100
                },
                new AdType()
                {
                    Name = "Premium", Index = 200
                },
                new AdType()
                {
                    Name = "Super Lux", Index = 400
                }
            };
            var categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Autos"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Motos"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Vehicles"
                },
            };
            var fakeAds = new List<Ad>()
            {
                new Ad()
                {
                    Id = 5,
                    Name = "Audi A6",
                    Type = adTypes[0],
                    PostedOn = DateTime.Now.AddDays(-6),
                    Price = 400,
                    Owner = new ApplicationUser(){UserName = "Gosho", Id = "123"},
                    Categories = new List<Category>()
                    {
                        categories[0], categories[2]
                    }

                },
                new Ad()
                {
                    Id = 6,
                    Name = "Opel",
                    Type = adTypes[1],
                    PostedOn = DateTime.Now.AddDays(-26),
                    Price = (decimal)800.73,
                    Owner = new ApplicationUser(){UserName = "Pesho", Id = "124"},
                    Categories = new List<Category>()
                    {
                        categories[1], categories[2]
                    }
                },
                 new Ad()
                {
                    Id = 7,
                    Name = "BMW",
                    Type = adTypes[2],
                    PostedOn = DateTime.Now.AddDays(-2),
                    Price = (decimal)1804.92,
                    Owner = new ApplicationUser(){UserName = "Ivan", Id = "125"},
                    Categories = new List<Category>()
                    {
                        categories[1]}
                },
                 new Ad()
                {
                    Id = 8,
                    Name = "Mercedes",
                    Type = adTypes[1],
                    PostedOn = DateTime.Now.AddDays(-3),
                    Price = (decimal)67893,
                    Owner = new ApplicationUser(){UserName = "Maria", Id = "126"},
                    Categories = new List<Category>()
                    {
                        categories[0], categories[2]
                    }
                },
            };
            this.AdRepositoryMock = new Mock<IRepository<Ad>>();

            this.AdRepositoryMock.Setup(r => r.All()).Returns(fakeAds.AsQueryable());

            this.AdRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => fakeAds.FirstOrDefault(ad => ad.Id == id));
        }

        private void SetUpFakeAdTypes()
        {
            var adTypes = new List<AdType>()
            {
                new AdType()
                {
                    Id = 1,
                    Name = "Normal",
                    Index = 100,
                    PricePerDay = 10
                },
                new AdType()
                {
                    Id = 2,
                    Name = "Premium",
                    Index = 200,
                    PricePerDay = 20
                },
                new AdType()
                {
                    Id = 3,
                    Name = "Super Lux",
                    Index = 400,
                    PricePerDay = 30
                }
            };
            this.AdTypeRepositoryMock = new Mock<IRepository<AdType>>();

            this.AdTypeRepositoryMock.Setup(r => r.All()).Returns(adTypes.AsQueryable());

            this.AdTypeRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => adTypes.FirstOrDefault(ad => ad.Id == id));
        }

    }
}
