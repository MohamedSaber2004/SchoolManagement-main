using MediatR;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Auth.Queries.GetUserId
{
    public class GetUserIdQueryHandler(IServiceManager _serviceManager) : IRequestHandler<GetUserIdQuery, string>
    {
        public async Task<string> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _serviceManager.AuthenticationService.GetUserIdByStudentIdAsync(request.StudentId);
        }
    }
}
