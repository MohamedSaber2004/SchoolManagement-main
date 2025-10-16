using AutoMapper;
using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Classes.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Classes.Commands.Update
{
    public class UpdateClassCommandHandler(IServiceManager _serviceManager,
                                           IValidator<UpdateClassCommand> _validator,
                                           IMapper _mapper) : IRequestHandler<UpdateClassCommand, ClassDto>
    {
        public async Task<ClassDto> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var updatedClass = _mapper.Map<UpdateClassCommand, UpdateClassDto>(request);
            return await _serviceManager.ClassService.UpdateAsync(updatedClass);
        }
    }
}
