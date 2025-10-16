using MediatR;
using School.Application.Features.Courses.DTOs;

namespace School.Application.Features.Courses.Queries.GetById
{
    public record GetCourseByIdQuery(int Id): IRequest<CourseDto>;
}
