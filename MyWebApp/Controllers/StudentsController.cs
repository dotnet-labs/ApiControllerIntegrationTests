namespace MyWebApp.Controllers;

[ApiController, Produces("application/json")]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ILogger<StudentsController> _logger;
    private readonly IStudentsService _studentsService;
    private readonly IHostEnvironment _environment;

    public StudentsController(ILogger<StudentsController> logger, IStudentsService studentsService, IHostEnvironment environment)
    {
        _logger = logger;
        _studentsService = studentsService;
        _environment = environment;
    }

    [HttpGet("{id:int}/name")]
    public ActionResult GetStudentName(int id)
    {
        _logger.LogInformation($"Environment: {_environment.EnvironmentName}");
        var name = _studentsService.GetStudentNameById(id);
        if (string.IsNullOrWhiteSpace(name))
        {
            return NoContent();
        }

        return Ok(name);
    }

    [HttpPost("")]
    public ActionResult Test([FromBody] MyRequest request)
    {
        _logger.LogInformation("{@request}", request);
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
    public string[] EmailNotificationToAddresses { get; set; } = { "VPFO-BO-AS_Processes@iowa.uiowa.edu" };
}