using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Controllers;
using OnlineShop.Services.Infrastructure;
using OnlineShop.Services.Models;
using OnlineShop.Services.Models.ViewModels;
using OnlineShop.Tests.Unit_Tests;

namespace OnlineShop.Tests
{
    [TestClass]
    public class AdsControllerTests
    {
        private MockContainer mocks;

        [TestInitialize]
        public void InitTests()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();
        }
        [TestMethod]
        public void GetAllAds_Should_Return_All_Ads_Sorted_By_TypeIndex()
        {
            var fakeAds = this.mocks.AdRepositoryMock.Object.All();
            var fakeUser = this.mocks.UserRepositoryMock.Object.All().FirstOrDefault();
            if (fakeUser == null)
            {
                Assert.Fail("Cannot perform test - no users available.");
            }

            var mockContext = new Mock<IOnlineShopData>();
            mockContext.Setup(c => c.Ads.All()).Returns(fakeAds);
            var fakeUsers = this.mocks.UserRepositoryMock.Object.All();
            mockContext.Setup(c => c.Users.All()).Returns(fakeUsers);
            
            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId()).Returns(fakeUser.Id);
            var adsController = new AdsController(mockContext.Object, mockIdProvider.Object);

            adsController.Request = new HttpRequestMessage();
            adsController.Configuration = new HttpConfiguration();

            var response = adsController.GetAds().ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var adsResponse = response.Content
                .ReadAsAsync<IEnumerable<AdViewModel>>()
                .Result.Select(a => a.Id)
                .ToList();

            var orderedFakeAds = fakeAds
                .OrderByDescending(a => a.Type.Index)
                .ThenByDescending(a => a.PostedOn)
                .Select(a => a.Id)
                .ToList();

            CollectionAssert.AreEqual(orderedFakeAds, adsResponse);
        }

        [TestMethod]
        public void CreateAd_Should_Successfully_Add_To_Repository()
        {
            var ads = new List<Ad>();
            var fakeUser = this.mocks.UserRepositoryMock.Object.All().FirstOrDefault();
            if (fakeUser == null)
            {
                Assert.Fail("Cannot perform test - no users available.");
            }
            this.mocks.AdRepositoryMock
                .Setup(r => r.Add(It.IsAny<Ad>()))
                .Callback((Ad ad) =>
                {
                    ad.Owner = fakeUser;
                    ads.Add(ad);
                });
            var mockContext = new Mock<IOnlineShopData>();
            mockContext.Setup(c => c.Ads).Returns(this.mocks.AdRepositoryMock.Object);
            var fakeCategories = this.mocks.CategoryRepositoryMock.Object.All();
            mockContext.Setup(c => c.Categories.All()).Returns(fakeCategories);
            mockContext.Setup(c => c.Categories.Find(It.IsAny<int>()))
                .Returns((int id) => fakeCategories.FirstOrDefault(c => c.Id == id));
            var fakeUsers = this.mocks.UserRepositoryMock.Object.All();
            mockContext.Setup(c => c.Users).Returns(this.mocks.UserRepositoryMock.Object);
            var fakeAdTypes = this.mocks.AdTypeRepositoryMock.Object.All();
            mockContext.Setup(c => c.AdTypes).Returns(this.mocks.AdTypeRepositoryMock.Object);

            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId()).Returns(fakeUser.Id);

            var adsController = new AdsController(mockContext.Object, mockIdProvider.Object);
            adsController.Request = new HttpRequestMessage();
            adsController.Configuration = new HttpConfiguration();

            var randomName = Guid.NewGuid().ToString();
            var newAd = new CreateAdBindingModel()
            {
                Name = randomName,
                Price = 999,
                TypeId = 1,
                Description = "Nothing to say",
                Categories = new[] {1, 2} 
            };
            var response = adsController.CreateAd(newAd).ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            mockContext.Verify(c=>c.SaveChanges(), Times.Once);
            Assert.AreEqual(1, ads.Count);
            Assert.AreEqual(newAd.Name, ads[0].Name);
        }

        [TestMethod]
        public void ClosingAd_As_Owner_Should_return_200OK()
        {
            var fakeAds = this.mocks.AdRepositoryMock.Object.All();
            var openAd = fakeAds.FirstOrDefault(ad => ad.Status == AdStatus.Open);
            if (openAd==null)
            {
                Assert.Fail("Cannot perform test - no open ads available.");
            }
            var adId = openAd.Id;
            var mockContext = new Mock<IOnlineShopData>();
            mockContext.Setup(c => c.Ads).Returns(this.mocks.AdRepositoryMock.Object);
            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId()).Returns(openAd.OwnerId);

            var adsController = new AdsController(mockContext.Object, mockIdProvider.Object);
            adsController.Request = new HttpRequestMessage();
            adsController.Configuration = new HttpConfiguration();

            var response = adsController.CloseAd(adId).ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreNotEqual(null, openAd.ClosedOn);
            Assert.AreEqual(AdStatus.Closed, openAd.Status);
        }

        [TestMethod]
        public void ClosingAd_As_Non_Owner_Should_return_400BadRequest()
        {
            var fakeAds = this.mocks.AdRepositoryMock.Object.All();
            var openAd = fakeAds.FirstOrDefault(ad => ad.Status == AdStatus.Open);
            if (openAd == null)
            {
                Assert.Fail("Cannot perform test - no open ads available.");
            }
            var adId = openAd.Id;
            ApplicationUser foreignUser = this.mocks.UserRepositoryMock.Object.All()
                .FirstOrDefault(u => u.Id != openAd.OwnerId);
            if (foreignUser == null)
            {
                Assert.Fail("Cannot perform test - need user who is non-owner of the ad.");
            }
            var mockIdProvider = new Mock<IUserIdProvider>();
            mockIdProvider.Setup(ip => ip.GetUserId()).Returns(foreignUser.Id);
            var mockContext = new Mock<IOnlineShopData>();
            mockContext.Setup(c => c.Ads).Returns(this.mocks.AdRepositoryMock.Object);
            var adsController = new AdsController(mockContext.Object, mockIdProvider.Object);
            adsController.Request = new HttpRequestMessage();
            adsController.Configuration = new HttpConfiguration();

            var response = adsController.CloseAd(adId).ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
            Assert.AreEqual(AdStatus.Open, openAd.Status);
        }
    }
}
