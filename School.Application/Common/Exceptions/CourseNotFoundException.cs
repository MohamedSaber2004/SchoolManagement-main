
namespace School.Application.Common.Exceptions
{
    public class CourseNotFoundException(int id): NotFoundExceptions($"Course with id '{id}' is not found.")
    {
    }
}
