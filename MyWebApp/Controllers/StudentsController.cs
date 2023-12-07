namespace MyWebApp.Controllers;

[ApiController, Produces("application/json")]
[Route("api/[controller]")]
public class StudentsController(ILogger<StudentsController> logger) : ControllerBase
{
    [HttpGet("{id:int}/name")]
    public ActionResult GetStudentName(int id, [FromServices] IHostEnvironment environment, [FromServices] IStudentsService studentsService)
    {
        logger.LogInformation("Environment: {environmentName}", environment.EnvironmentName);
        var name = studentsService.GetStudentNameById(id);
        if (string.IsNullOrWhiteSpace(name))
        {
            return NoContent();
        }

        return Ok(name);
    }

    [HttpPost("")]
    public ActionResult Test([FromBody] MyRequest request)
    {
        logger.LogInformation("{@request}", request);
        request.PickupDateTime = request.PickupDateTime.Kind != DateTimeKind.Local ? request.PickupDateTime.ToLocalTime() : request.PickupDateTime;
        return Ok(request);
    }

    public static void ExplicitlyMarkDateTimeAsLocal(MyRequest v)
    {
        var props = typeof(MyRequest).GetProperties();
        foreach (var p in props)
        {
            if (p.PropertyType == typeof(DateTime))
            {
                var d = (DateTime)(p.GetValue(v) ?? DateTime.MinValue);
                p.SetValue(v, DateTime.SpecifyKind(d, DateTimeKind.Local));
            }
        }
    }
}


public class MyRequest
{
    public int Id { get; set; }
    public bool TryMe { get; set; } = false;
    public bool TryMe2 { get; set; } = true;
    public DateTime PickupDateTime { get; set; }
    public string[] EmailNotificationToAddresses { get; set; } = { "es@uiowa.edu" };
}