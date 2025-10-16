using MediatR;
using School.Application.Features.Auth.DTOs;

namespace School.Application.Features.Auth.Queries.GetCurrentUser
{
    public record GetCurrentUser(string email): IRequest<UserDto>;
}
