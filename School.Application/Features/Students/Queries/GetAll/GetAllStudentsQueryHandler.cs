using MediatR;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Students.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Students.Queries.GetAll
{
    public class GetAllStudentsQueryHandler(IServiceManager _serviceManager) : IRequestHandler<GetAllStudentsQuery, PaginatedResult<StudentDto>>
    {
        public async Task<PaginatedResult<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken) 
            => await _serviceManager.StudentService.GetAllAsync(request.queryParams);
    }
}
