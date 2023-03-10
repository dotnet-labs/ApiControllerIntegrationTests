namespace MyWebApp.IntegrationTests;

[TestClass]
public class ValuesControllerTests
{
    [TestMethod]
    public async Task ShouldReturnSuccessResponse()
    {
        await using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateDefaultClient();
        var response = await client.GetAsync("api/values");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var json = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("[\"value1\",\"value2\"]", json);
    }
}