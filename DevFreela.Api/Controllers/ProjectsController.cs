using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevFreela.Api.Models;
using Microsoft.Extensions.Options;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.InputModels;
using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
namespace DevFreela.Api.Controllers
{
  [Route("api/projects")]
  public class ProjectsController : ControllerBase
  {
    private readonly IProjectService _projectService;
    public ProjectsController(IProjectService projectService)
    {
      _projectService = projectService;
    }

    [HttpGet]
    public IActionResult Get(string query)
    {
      var projects = _projectService.GetAll();
      return Ok(projects);
    }

    [HttpGet("{Id}")]
    public IActionResult GetById(int Id)
    {
      var project = _projectService.GetById(Id);
      if (project == null) return NotFound();
      return Ok(project);
    }
    [HttpPost]
    public IActionResult Post([FromBody] NewProjectInputModel inputModel)
    {
      var id = _projectService.Create(inputModel);
      return CreatedAtAction(nameof(GetById), new { id = id }, inputModel);
    }
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdateProjectInputModel inputModel)
    {
      if (inputModel.Description.Length > 200)
      {
        return BadRequest();
      }
      _projectService.Update(inputModel);
      return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      _projectService.Delete(id);
      return NoContent();
    }
    [HttpPost("{id}/comments")]
    public IActionResult PostComment(int id, [FromBody] CreateCommentInputModel inputModel)
    {
      _projectService.CreateComment(inputModel);
      return NoContent();
    }
    [HttpPut("{id}/start")]
    public IActionResult Start(int id)
    {
      _projectService.Start(id);
      return NoContent();
    }
    [HttpPut("{id}/finish")]
    public IActionResult Finish(int id)
    {
      _projectService.Finish(id);
      return NoContent();
    }
  }
}