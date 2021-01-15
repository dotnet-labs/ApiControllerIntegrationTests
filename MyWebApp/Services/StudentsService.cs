using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MyWebApp.Services
{
    public interface IStudentsService
    {
        string GetStudentNameById(int studentId);
    }

    public class StudentsService : IStudentsService
    {
        private readonly ILogger<StudentsService> _logger;
        private readonly Dictionary<int, string> _studentNamesDictionary = new Dictionary<int, string>
        {
            {123, "Sample Name 1"},
            {124, "Sample Name 2"},
            {125, "Sample Name 3"},
            {126, "Sample Name 4"},
        };
        public StudentsService(ILogger<StudentsService> logger)
        {
            _logger = logger;
        }

        public string GetStudentNameById(int studentId)
        {
            _studentNamesDictionary.TryGetValue(studentId, out var name);
            return name;
        }
    }

}
