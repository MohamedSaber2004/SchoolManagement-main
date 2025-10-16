using MediatR;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Students.DTOs;

namespace School.Application.Features.Students.Queries.GetAll
{
    public record GetAllStudentsQuery(StudentQueryParams queryParams): IRequest<PaginatedResult<StudentDto>>;
}
