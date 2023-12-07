using System.Text;

namespace MyWebApp.IntegrationTests;

[TestClass]
public class StudentsControllerTests
{
    private static TestContext _testContext = null!;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
    }

    [TestMethod]
    public async Task ShouldReturnSuccessResponse()
    {
        Console.WriteLine(_testContext.TestName);

        await using var factory = new WebApplicationFactory<Startup>();
        using var client = factory.CreateClient();
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

        await using var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("https_port", "5001").UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IStudentsService, MockStudentService>();
            });
        });

        using var client = factory.CreateClient();
        var response = await client.GetAsync("api/students/123/name");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"Test Name 1\"", result);
    }

    [TestMethod]
    public async Task ShouldReturnSuccessResponse2()
    {
        Console.WriteLine(_testContext.TestName);

        await using var factory = new WebApplicationFactory<Startup>();
        using var client = factory.CreateClient();
        var response = await client.GetAsync("api/students/123/name");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"Sample Name 1\"", result);
    }

    [TestMethod]
    public async Task ShouldReturnSuccessResponse3()
    {
        Console.WriteLine(_testContext.TestName);

        await using var factory = new WebApplicationFactory<Startup>();
        using var client = factory.CreateClient();
        var a = new MyRequest
        {
            Id = 12,
            EmailNotificationToAddresses = ["test"]
        };
        var response = await client.PostAsJsonAsync("api/students/", a);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("{\"id\":12,\"tryMe\":false,\"tryMe2\":true,\"pickupDateTime\":\"0001-01-01T00:00:00-06:00\",\"emailNotificationToAddresses\":[\"test\"]}", result);
    }

    [TestMethod]
    public async Task DateJsonStringConversionTest()
    {
        Console.WriteLine(_testContext.TestName);

        await using var factory = new WebApplicationFactory<Startup>();
        using var client = factory.CreateClient();

        var response = await client.PostAsync("api/students/", new StringContent("""{"id":12,"pickupDateTime":"2023-03-10T15:00:00.000Z"}""", Encoding.UTF8, "application/json"));

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("{\"id\":12,\"tryMe\":false,\"tryMe2\":true,\"pickupDateTime\":\"2023-03-10T09:00:00-06:00\",\"emailNotificationToAddresses\":[\"es@uiowa.edu\"]}", result);
    }
}

public class MockStudentService : IStudentsService
{
    public string GetStudentNameById(int studentId)
    {
        return "Test Name 1";
    }
}