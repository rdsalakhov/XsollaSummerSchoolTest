using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XsollaSummerSchoolTest.Controllers;
using XsollaSummerSchoolTest.Models;

namespace NewsApiTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GetOneNewsItem_OK()
        {
            var controller = new NewsItemsController();
            var response = controller.GetNewsItem(1);
            var result = response as OkNegotiatedContentResult<NewsItemSend>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void GetOneNewsItem_NotFound()
        {
            var controller = new NewsItemsController();
            var response = controller.GetNewsItem(-1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetAllNewsItems_OK()
        {
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var response = controller.GetNewsItemSet();
            IEnumerable<NewsItemSend> newsItems;
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<NewsItemSend>>(out newsItems));
            Assert.AreEqual(newsItems.First().Id, 1);
        }


    }
}
