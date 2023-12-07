namespace MyWebApp.IntegrationTests;

[TestClass]
public class WeatherForecastControllerTests
{
    private static TestContext _testContext = null!;
    private static WebApplicationFactory<Startup> _factory = null!;

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
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = JsonSerializer.Deserialize<WeatherForecast[]>(json, options);
        Assert.AreEqual(5, result?.Length);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }
}