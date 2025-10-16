using MediatR;
using School.Application.Features.Students.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Students.Queries.GetById
{
    public class GetStudentByIdQueryHandler(IServiceManager _serviceManager) : IRequestHandler<GetStudentByIdQuery, StudentDto>
    {
        public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
            => await _serviceManager.StudentService.GetByIdAsync(request.Id);
    }
}
