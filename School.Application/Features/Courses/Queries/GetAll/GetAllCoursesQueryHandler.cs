using MediatR;
using School.Application.Common.Models;
using School.Application.Features.Courses.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Courses.Queries.GetAll
{
    public class GetAllCoursesQueryHandler(IServiceManager _serviceManager) : IRequestHandler<GetAllCoursesQuery, PaginatedResult<CourseDto>>
    {
        public async Task<PaginatedResult<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
            => await _serviceManager.CourseService.GetAllAsync(request.queryParams);
    }
}
