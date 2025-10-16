using MediatR;

namespace School.Application.Features.Auth.Queries.GetUserId
{
    public record GetUserIdQuery(int StudentId) : IRequest<string>;
}
