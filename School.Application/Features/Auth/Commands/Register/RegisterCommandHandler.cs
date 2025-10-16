using AutoMapper;
using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Auth.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler(IServiceManager _serviceManager,
                                        IValidator<RegisterCommand> _validator,
                                        IMapper _mapper) : IRequestHandler<RegisterCommand, UserDto>
    {
        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request,cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var registerDto = _mapper.Map<RegisterDto>(request);

            return await _serviceManager.AuthenticationService.RegisterAsync(registerDto);
        }
    }
}
