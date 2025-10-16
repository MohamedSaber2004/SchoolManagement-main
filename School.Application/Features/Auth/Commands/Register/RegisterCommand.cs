using MediatR;
using School.Application.Features.Auth.DTOs;

namespace School.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(string Email,string DisplayName, string UserName, string Phone, string Password): IRequest<UserDto>;
}
