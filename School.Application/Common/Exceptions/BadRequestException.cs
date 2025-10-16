namespace School.Application.Common.Exceptions
{
    public sealed class BadRequestException(List<string> errors): Exception("validation failed")
    {
        public List<string> Errors { get; } = errors;
    }
}
