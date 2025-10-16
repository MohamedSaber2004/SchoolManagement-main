using Microsoft.AspNetCore.Mvc;
using School.Api.ErrorModels;

namespace School.Api.Extensions
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationErrorsResponse(ActionContext Context)
        {
            var Errors = Context.ModelState.Where(M => M.Value!.Errors.Any())
                                .Select(M => new ValidationError()
                                {
                                    Field = M.Key,
                                    Errors = M.Value!.Errors.Select(E => E.ErrorMessage)
                                }).ToList();

            var response = new ValidationErrorToReturn() { ValidationErrors = Errors };

            return new BadRequestObjectResult(response);
        }
    }
}
