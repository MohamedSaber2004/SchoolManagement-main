using MediatR;
using School.Application.Features.Auth.DTOs;

namespace School.Application.Features.Auth.Queries.Login
{
    public record LoginQuery(string Email, string Password): IRequest<UserDto>;
}
