using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Classes.Commands.Create;
using School.Application.Features.Classes.Commands.Delete;
using School.Application.Features.Classes.Commands.Update;
using School.Application.Features.Classes.DTOs;
using School.Application.Features.Classes.Queries.GetAll;
using School.Application.Features.Classes.Queries.GetById;

namespace School.Api.Controllers
{
    public class ClassesController(IMediator _mediator): ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<ActionResult<PaginatedResult<ClassDto>>> GetAll([FromQuery] ClassQueryParams queryParams)
        {
            var query = new GetAllClassesQuery(queryParams);
            var classes = await _mediator.Send(query);
            return Ok(classes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<ActionResult<ClassDto>> GetById(int id)
        {
            var query = new GetClassByIdQuery(id);
            var Class = await _mediator.Send(query);
            return Ok(Class);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ClassDto>> Create(CreateClassCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<ActionResult<ClassDto>> Update(int id, UpdateClassCommand command)
        {
            if(id != command.Id)
                return BadRequest("Id in route does not match Id in command body");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var command = new DeleteClassCommand(id);
            var result = await _mediator.Send(command);
            return result ? NoContent(): NotFound();
        }
    }
}
