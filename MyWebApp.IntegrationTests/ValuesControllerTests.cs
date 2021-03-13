using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyWebApp.IntegrationTests
{
    [TestClass]
    public class ValuesControllerTests
    {
        [TestMethod]
        public async Task ShouldReturnSuccessResponse()
        {
            using var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateDefaultClient();
            var response = await client.GetAsync("api/values");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var json = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("[\"value1\",\"value2\"]", json);
        }
    }
}
