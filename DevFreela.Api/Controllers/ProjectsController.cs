using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevFreela.Api.Models;
using Microsoft.Extensions.Options;
using DevFreela.Application.InputModels;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.StartProject;
using DevFreela.Application.Commands.FinishProject;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Application.Queries.GetProjectById;
using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace DevFreela.Api.Controllers
{
  [Route("api/projects")]
  public class ProjectsController : ControllerBase
  {
    private readonly IMediator _mediator;
    public ProjectsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "client, freelancer")]
    public async Task<IActionResult> Get(string query)
    {
      var getAllProject = new GetAllProjectsQuery(query);
      var projects = await _mediator.Send(getAllProject);
      return Ok(projects);
    }

    [HttpGet("{Id}")]
    [Authorize(Roles = "client, freelancer")]
    public async Task<IActionResult> GetById(int Id)
    {
      var query = new GetProjectByIdQuery(Id);
      var project = await _mediator.Send(query);
      if (project == null) return NotFound();
      return Ok(project);
    }
    [HttpPost]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> Post([FromBody] CreateProjectCommand command)
    {
      var id = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetById), new { id = id }, command);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "client")]

    public async Task<IActionResult> Put(int id, [FromBody] UpdateProjectCommand command)
    {
      if (command.Description.Length > 200)
      {
        return BadRequest();
      }
      await _mediator.Send(command);
      return NoContent();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "client")]

    public async Task<IActionResult> Delete(int id)
    {
      var command = new DeleteProjectCommand(id);
      await _mediator.Send(command);
      return NoContent();
    }
    [HttpPost("{id}/comments")]
    [Authorize(Roles = "client, freelancer")]

    public async Task<IActionResult> PostComment(int id, [FromBody] CreateCommentCommand command)
    {
      await _mediator.Send(command);
      return NoContent();
    }
    [HttpPut("{id}/start")]
    [Authorize(Roles = "client")]

    public async Task<IActionResult> Start(int id)
    {
      var command = new StartProjectCommand(id);
      await _mediator.Send(command);
      return NoContent();
    }
    [HttpPut("{id}/finish")]
    [Authorize(Roles = "client")]

    public async Task<IActionResult> Finish(int id)
    {
      var command = new FinishProjectCommand(id);
      await _mediator.Send(command);
      return NoContent();
    }
  }
}