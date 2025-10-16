namespace School.Api.ErrorModels
{
    public class ErrorToReturn
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string>? Errors { get; set; } = [];
    }
}
