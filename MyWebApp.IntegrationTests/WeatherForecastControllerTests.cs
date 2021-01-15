using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyWebApp.IntegrationTests
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        private static TestContext _testContext;
        private static WebApplicationFactory<Startup> _factory;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                    builder.UseSetting("https_port", "5001").UseEnvironment("Testing")
                );
        }

        [TestMethod]
        public async Task ShouldReturnSuccessResponse()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("WeatherForecast");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WeatherForecast[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.AreEqual(5, result?.Length);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _factory.Dispose();
        }
    }
}
