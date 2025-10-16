using AutoMapper;
using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Classes.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Classes.Commands.Create
{
    public class CreateClassCommandHandler(IServiceManager _serviceManager,
                                      IValidator<CreateClassCommand> _validator,
                                      IMapper _mapper) : IRequestHandler<CreateClassCommand, ClassDto>
    {
        public async Task<ClassDto> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var Class = _mapper.Map<CreateClassCommand, CreateClassDto>(request);
            return await _serviceManager.ClassService.CreateAsync(Class);
        }
    }
}
