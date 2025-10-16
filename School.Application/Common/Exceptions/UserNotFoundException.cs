namespace School.Application.Common.Exceptions
{
    public sealed class UserNotFoundException(string email) : NotFoundExceptions($"User with email '{email}' is not found.")
    {
    }
}
