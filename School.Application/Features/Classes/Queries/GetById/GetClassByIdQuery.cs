using MediatR;
using School.Application.Features.Classes.DTOs;

namespace School.Application.Features.Classes.Queries.GetById
{
    public record GetClassByIdQuery(int Id) : IRequest<ClassDto>;
}
