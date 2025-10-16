using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Chat.Dtos;
using School.Application.Features.Students.Commands.Create;
using School.Application.Features.Students.Commands.Delete;
using School.Application.Features.Students.Commands.Update;
using School.Application.Features.Students.DTOs;
using School.Application.Features.Students.Queries.GetAll;
using School.Application.Features.Students.Queries.GetById;
using School.Infrastructure.Identity;
using System.Security.Claims;

namespace School.Api.Controllers
{
    public class StudentsController(IMediator _mediator, UserManager<ApplicationUser> _userManager): ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<PaginatedResult<StudentDto>>> GetAll([FromQuery]StudentQueryParams queryParams)
        {
            var query = new GetAllStudentsQuery(queryParams);
            var students = await _mediator.Send(query);
            return Ok(students);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<ActionResult<StudentDto>> GetById(int id)
        {
            var query = new GetStudentByIdQuery(id);
            var student = await _mediator.Send(query);
            return Ok(student);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDto>> Create(CreateStudentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDto>> Update(int id, UpdateStudentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Id in route does not match Id in command body");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var command = new DeleteStudentCommand(id);
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Student")]
        [HttpPost("token")]
        public async Task<ActionResult> RegisterDeviceToken([FromBody] RegisterDeviceTokenDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found");

            user.FirebaseDeviceToken = dto.DeviceToken;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Device token registered successfully" });
        }
    }
}
