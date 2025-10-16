namespace School.Application.Common.Exceptions
{
    public class UnauthorizedException(string message = "Invalid email or password.") : Exception(message)
    {
    }
}
