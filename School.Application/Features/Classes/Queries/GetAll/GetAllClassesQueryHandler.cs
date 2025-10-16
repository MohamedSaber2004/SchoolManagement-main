using MediatR;
using School.Application.Common.Models;
using School.Application.Features.Classes.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Classes.Queries.GetAll
{
    public class GetAllClassesQueryHandler(IServiceManager _serviceManager) : IRequestHandler<GetAllClassesQuery, PaginatedResult<ClassDto>>
    {
        public Task<PaginatedResult<ClassDto>> Handle(GetAllClassesQuery request, CancellationToken cancellationToken) 
            => _serviceManager.ClassService.GetAllAsync(request.classQueryParams);
    }
}
