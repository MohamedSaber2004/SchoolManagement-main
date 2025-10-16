using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Common.Models;
using School.Application.Common.Models.QueryParams;
using School.Application.Features.Courses.Commands.Create;
using School.Application.Features.Courses.Commands.Delete;
using School.Application.Features.Courses.Commands.Update;
using School.Application.Features.Courses.DTOs;
using School.Application.Features.Courses.Queries.GetAll;
using School.Application.Features.Courses.Queries.GetById;

namespace School.Api.Controllers
{
    public class CoursesController(IMediator _mediator):ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<ActionResult<PaginatedResult<CourseDto>>> GetAll([FromQuery] CourseQueryParams queryParams)
        {
            var query = new GetAllCoursesQuery(queryParams);
            var courses = await _mediator.Send(query);
            return Ok(courses);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<ActionResult<CourseDto>> GetById(int id)
        {
            var query = new GetCourseByIdQuery(id);
            var course = await _mediator.Send(query);
            return Ok(course);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CourseDto>> Create(CreateCourseCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var command = new DeleteCourseCommand(id);
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();   
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<CourseDto>> Update(int id, UpdateCourseCommand command)
        {
            if (id != command.Id)
                return BadRequest("Course Id mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
