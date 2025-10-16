using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Features.Auth.Commands.CreateStudent;
using School.Application.Features.Auth.DTOs;
using System.Security.Claims;

namespace School.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController(IMediator _mediator): ApiBaseController
    {
        [HttpPost("create-student-account")]
        public async Task<ActionResult<StudentAccountDto>> CreateStudentAccount([FromBody] CreateStudentAccountDto dto)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var command = new CreateStudentCommand(dto, adminId);
            return await _mediator.Send(command);
        }
    }
}
