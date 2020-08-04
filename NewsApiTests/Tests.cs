using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XsollaSummerSchoolTest;
using XsollaSummerSchoolTest.Controllers;
using XsollaSummerSchoolTest.Models;

namespace NewsApiTests
{
    // good idea is to use mock data repository for testing (https://docs.microsoft.com/ru-ru/aspnet/web-api/overview/testing-and-debugging/unit-testing-with-aspnet-web-api)
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GetOneNewsItem_OK()
        {
            // Arrange
            var controller = new NewsItemsController();

            // Act
            var response = controller.GetNewsItem(1);
            var result = response as OkNegotiatedContentResult<NewsItemSend>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content); // nit: might be a good idea to check if item corresponds to the one requested
        }

        [TestMethod]
        public void GetOneNewsItem_FailNotFound()
        {
            // Arrange
            var controller = new NewsItemsController();

            // Act
            var response = controller.GetNewsItem(-1);
            
            // Assert
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetAllNewsItems_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetNewsItemSet();

            // Assert
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<NewsItemSend>>(out var newsItems));
            // nit: might be a good idea to check at least the length of the array, not just the success of cast
        }

        [TestMethod]
        public void GetNewsItemsByCategory_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetNewsItemSet("Искусство");

            // Assert
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<NewsItemSend>>(out var newsItems));
            Assert.AreEqual("Искусство", newsItems.First().Category);
        }

        [TestMethod]
        public void GetTopRatedNewsItems_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            double leastAverage = 7;

            // Act
            var response = controller.GetNewsItemSet(leastAverage);
            
            // Assert
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<NewsItemSend>>(out var newsItems));
            Assert.IsTrue(newsItems.First().TotalRate / newsItems.First().RateCount >= leastAverage);
        }

        [TestMethod]
        public void PutNewsItem_OK() // difficult to understand whether this test checks the scenario if a client behavior is correct
        {
            // Arrange
            var controller = new NewsItemsController();

            // Act
            var response = (StatusCodeResult)controller.PutNewsItem(1, new NewsItem
            {
                Headline = "Test put", 
                Body = "Test put", 
                Category = "Test"
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            // might be a good idea to check whether the operation was successful or not
        }

        [TestMethod]
        public void PutNewsItem_FailNotFound()
        {
            // Arrange
            var controller = new NewsItemsController();

            // Act
            var response = controller.PutNewsItem(-4, new NewsItem
            {
                Headline = "Test put", 
                Body = "Test put", 
                Category = "Test"
            });
            var result = response as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutNewsItem_FailBadRequest()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.ModelState.AddModelError("Body", "The Body field is required.");

            // Act
            var response = controller.PutNewsItem(1, new NewsItem { Headline = "Test put", Category = "Test" });
            var result = response as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            
        }

        [TestMethod]
        public void PostNewsItem_FailBadRequest()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            //Controller creation has to be reused
            controller.ModelState.AddModelError("Body", "The Body field is required.");

            // Act
            var response = controller.PostNewsItem(new NewsItem
            {
                Headline = "Test post", 
                Category = "Test"
            });
            var result = response as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);           
        }

        [TestMethod]
        public void PostNewsItem_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.PostNewsItem(new NewsItem
            {
                Headline = "Test post", 
                Body = "Test post", 
                Category = "Test"
            });
            var result = response as CreatedAtRouteNegotiatedContentResult<NewsItemSend>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("DefaultApi", result.RouteName);
            Assert.IsNotNull(result.RouteValues["id"]);
        }

        [TestMethod]
        public void DeleteNewsItem_FailNotFound()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.DeleteNewsItem(-1);
            var result = response as NotFoundResult;

            // Assert            
            Assert.IsNotNull(result);          
        }

        [TestMethod]
        public void DeleteNewsItem_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.DeleteNewsItem(1);
            var result = response as OkNegotiatedContentResult<NewsItemSend>;

            // Assert            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostRate_FailNotFound()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.PostRate(-3, 8);

            // Assert            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void PostRate_FailBadRequest()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.PostRate(2, 123);

            // Assert            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void PostRate_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("sessionString", "i71S2pE8tJ5LV0F658HCXK2zF57M3L"); // nit: might be a good idea to generate that one prior 
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/"); 

            // Act
            var response = controller.PostRate(2, 8);

            // Assert            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // nit: might be a good idea to check if that rate changed anything
        }

        [TestMethod]
        public void PostRate_FailForbidden()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("Cookie", "sessionString=i81S2p54tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");
            controller.PostRate(2, 8);

            // Act
            var response = controller.PostRate(2, 8);

            // Assert            
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }


        [TestMethod]
        public void DeleteRate_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("Cookie", "sessionString=i71S2pE4tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");
            controller.PostRate(2, 8);

            // Act
            var response = controller.DeleteRate(2);

            // Assert            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // nit: might be a good idea to check if that rate cancellation changed anything
        }

        [TestMethod]
        public void DeleteRate_FailForbidden()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("Cookie", "sessionString=i81S2pE4tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");

            // Act
            var response = controller.DeleteRate(2);

            // Assert            
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void DeleteRate_FailNotFound()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("Cookie", "sessionString=i81S2pE4tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");

            // Act
            var response = controller.DeleteRate(-1);

            // Assert            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    
}
