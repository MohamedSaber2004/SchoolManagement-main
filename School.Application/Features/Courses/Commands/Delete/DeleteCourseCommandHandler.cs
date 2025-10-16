using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Courses.Commands.Delete
{
    public class DeleteCourseCommandHandler(IServiceManager _serviceManager,
                                            IValidator<DeleteCourseCommand> _validator) : IRequestHandler<DeleteCourseCommand, bool>
    {
        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            return await _serviceManager.CourseService.DeleteAsync(request.Id);
        }
    }
}
