
namespace School.Application.Common.Exceptions
{
    public sealed class ClassNotFoundException(int id): NotFoundExceptions($"Class with id {id} not found.")
    {
    }
}
