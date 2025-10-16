using MediatR;
using School.Application.Features.Courses.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Courses.Queries.GetById
{
    public class GetCourseByIdQueryHandler(IServiceManager _serviceManager) : IRequestHandler<GetCourseByIdQuery, CourseDto>
    {
        public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
            => await _serviceManager.CourseService.GetByIdAsync(request.Id);
    }
}
