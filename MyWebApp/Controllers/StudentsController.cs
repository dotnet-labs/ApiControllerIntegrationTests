using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyWebApp.Services;

namespace MyWebApp.Controllers
{
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
    }
}
