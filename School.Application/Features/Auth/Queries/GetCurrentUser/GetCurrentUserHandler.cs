using MediatR;
using School.Application.Features.Auth.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Auth.Queries.GetCurrentUser
{
    public class GetCurrentUserHandler(IServiceManager _serviceManager) : IRequestHandler<GetCurrentUser, UserDto>
    {
        public Task<UserDto> Handle(GetCurrentUser request, CancellationToken cancellationToken)
            => _serviceManager.AuthenticationService.GetCurrentUserAsync(request.email);
    }
}
