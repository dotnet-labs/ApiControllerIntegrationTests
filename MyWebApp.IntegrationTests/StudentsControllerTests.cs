using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebApp.Services;

namespace MyWebApp.IntegrationTests
{
    [TestClass]
    public class StudentsControllerTests
    {
        private static TestContext _testContext;
        private static WebApplicationFactory<Startup> _factory;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
            _factory = new WebApplicationFactory<Startup>();
        }

        [TestMethod]
        public async Task ShouldReturnSuccessResponse()
        {
            Console.WriteLine(_testContext.TestName);

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/students/123/name");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("\"Sample Name 1\"", result);
        }

        [TestMethod]
        public async Task ShouldReturnSuccessResponseWithMockService()
        {
            Console.WriteLine(_testContext.TestName);

            _factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("https_port", "5001").UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IStudentsService, MockStudentService>();
                });
            });

            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/students/123/name");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("\"Test Name 1\"", result);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _factory.Dispose();
        }
    }

    public class MockStudentService : IStudentsService
    {
        public string GetStudentNameById(int studentId)
        {
            return "Test Name 1";
        }
    }
}
