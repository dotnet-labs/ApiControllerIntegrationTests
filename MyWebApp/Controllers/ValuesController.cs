namespace MyWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    [HttpGet("")]
    public ActionResult GetValues()
    {
        var values = new[] { "value1", "value2" };
        return Ok(values);
    }
}