using MediatR;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Classes.DTOs;

namespace School.Application.Features.Classes.Queries.GetAll
{
    public record GetAllClassesQuery(ClassQueryParams classQueryParams): IRequest<PaginatedResult<ClassDto>>;
}
