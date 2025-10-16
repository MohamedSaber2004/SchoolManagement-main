using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Students.Commands.Delete
{
    public class DeleteStudentCommandHandler(IServiceManager _serviceManager,
                                      IValidator<DeleteStudentCommand> _validator) : IRequestHandler<DeleteStudentCommand, bool>
    {
        public Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            return _serviceManager.StudentService.DeleteAsync(request.Id);
        }
    }
}
