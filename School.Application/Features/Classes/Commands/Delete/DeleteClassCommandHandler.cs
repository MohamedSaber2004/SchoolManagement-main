using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Classes.Commands.Delete
{
    public class DeleteClassCommandHandler(IServiceManager _serviceManager,
                                           IValidator<DeleteClassCommand> _validator) : IRequestHandler<DeleteClassCommand, bool>
    {
        public async Task<bool> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            return await _serviceManager.ClassService.DeleteAsync(request.Id);
        }
    }
}
