namespace MyWebApp.Services;

public interface IStudentsService
{
    string GetStudentNameById(int studentId);
}

public class StudentsService(ILogger<StudentsService> logger) : IStudentsService
{
    private readonly Dictionary<int, string> _studentNamesDictionary = new()
    {
        {123, "Sample Name 1"},
        {124, "Sample Name 2"},
        {125, "Sample Name 3"},
        {126, "Sample Name 4"},
    };

    public string GetStudentNameById(int studentId)
    {
        logger.LogInformation("\t getting student name for ID={id}", studentId);
        _studentNamesDictionary.TryGetValue(studentId, out var name);
        return name ?? string.Empty;
    }
}