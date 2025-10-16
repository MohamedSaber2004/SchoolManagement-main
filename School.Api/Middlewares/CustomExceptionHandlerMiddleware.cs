using School.Api.ErrorModels;
using School.Application.Common.Exceptions;

namespace School.Api.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                await HandleNotFoundEndPointAsync(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something Went Wrong");

                await HandleExceptionsAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionsAsync(HttpContext httpContext, Exception ex)
        {
            var response = new ErrorToReturn()
            {
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                BadRequestException badRequestException => GetBadRequestErrors(badRequestException, response),
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                NotFoundExceptions => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = response.StatusCode;
            httpContext.Response.ContentType = "application/json";
            
            await httpContext.Response.WriteAsJsonAsync(response);
        }

        private static int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn response)
        {
            response.Errors = badRequestException.Errors;
            return StatusCodes.Status400BadRequest;
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found"
                };

                httpContext.Response.ContentType = "application/json";
                
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
