using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Features.Auth.Commands.Register;
using School.Application.Features.Auth.DTOs;
using School.Application.Features.Auth.Queries.GetCurrentUser;
using School.Application.Features.Auth.Queries.GetUserId;
using School.Application.Features.Auth.Queries.Login;

namespace School.Api.Controllers
{
    public class AuthController(IMediator _mediator): ApiBaseController
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(LoginQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = GetEmailFromToken();
            var query = new GetCurrentUser(email);
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("GetUserId/{StudentId}")]
        public async Task<string> GetUserIdByStudentId(int StudentId)
        {
            var query = new GetUserIdQuery(StudentId);
            var userId = await _mediator.Send(query);
            return userId;
        }
    }
}
