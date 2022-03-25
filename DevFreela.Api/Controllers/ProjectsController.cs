using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevFreela.Api.Models;
using Microsoft.Extensions.Options;

namespace DevFreela.Api.Controllers
{
  [Route("api/projects")]
  public class ProjectsController : ControllerBase
  {
    private readonly OpeningTimeOption _option;
    public ProjectsController(IOptions<OpeningTimeOption> option)
    {
      _option = option.Value;
    }

    [HttpGet]
    public IActionResult Get(string query)
    {
      return Ok();
    }

    [HttpGet("{Id}")]
    public IActionResult GetById(int Id)
    {
      return Ok();
    }
    [HttpPost]
    public IActionResult Post([FromBody] CreateProjectModel createProject)
    {
      return CreatedAtAction(nameof(GetById), new { id = createProject.Id }, createProject);
    }
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdateProjectModel updateProjectModel)
    {
      if (updateProjectModel.Description.Length > 200)
      {
        return BadRequest();
      }
      return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      return NoContent();
    }
    [HttpPost("{id}/comments")]
    public IActionResult PostComment(int id, [FromBody] CreateCommentModel createCommentModel)
    {
      return NoContent();
    }
    [HttpPut("{id}/start")]
    public IActionResult Start(int id)
    {
      return NoContent();
    }
    [HttpPut("{id}/finish")]
    public IActionResult Finish(int id)
    {
      return NoContent();
    }
  }
}