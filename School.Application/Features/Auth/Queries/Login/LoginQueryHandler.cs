using AutoMapper;
using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Auth.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Auth.Queries.Login
{
    public class LoginQueryHandler(IAuthenticationService _authenticationService,
                                   IValidator<LoginQuery> _validator,
                                   IMapper _mapper) : IRequestHandler<LoginQuery, UserDto>
    {
        public async Task<UserDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var loginDto = _mapper.Map<LoginDto>(request);

            return await _authenticationService.LoginAsync(loginDto);
        }
    }
}
