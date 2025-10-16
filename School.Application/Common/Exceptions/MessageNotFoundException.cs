namespace School.Application.Common.Exceptions
{
    public sealed class MessageNotFoundException(int messageId): NotFoundExceptions($"Message with Id {messageId} not found.")
    {
    }
}
