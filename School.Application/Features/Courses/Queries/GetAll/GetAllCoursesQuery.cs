using MediatR;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Courses.DTOs;

namespace School.Application.Features.Courses.Queries.GetAll
{
    public record GetAllCoursesQuery(CourseQueryParams queryParams): IRequest<PaginatedResult<CourseDto>>;
}
