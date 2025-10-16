namespace School.Application.Common.Exceptions
{
    public sealed class StudentNotFoundException(int id): NotFoundExceptions($"Student with id '{id}' is not found.")
    {
    }
}
