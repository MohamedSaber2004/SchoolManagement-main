using AutoMapper;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Classes.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Classes.Queries.GetById
{
    public class GetClassByIdQueryHandler(IServiceManager _serviceManager) : IRequestHandler<GetClassByIdQuery, ClassDto>
    {
        public async Task<ClassDto> Handle(GetClassByIdQuery request, CancellationToken cancellationToken) 
            => await _serviceManager.ClassService.GetByIdAsync(request.Id);
    }
}
