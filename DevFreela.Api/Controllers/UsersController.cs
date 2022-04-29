using Microsoft.AspNetCore.Mvc;
using DevFreela.Api.Models;
using DevFreela.Application.InputModels;
using DevFreela.Application.Commands.CreateUser;
using MediatR;
using System.Threading.Tasks;
using DevFreela.Application.Queries.GetUserById;
namespace DevFreela.Api.Controllers
{
  [Route("api/users")]
  public class UsersController : ControllerBase
  {
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
      _mediator = mediator;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var query = new GetUserByIdQuery(id);
      var user = await _mediator.Send(query);

      if (user == null)
      {
        return NotFound();
      }

      return Ok(user);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
    {
      var id = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetById), new { id = id }, command);
    }
    // api/users/1/login   
    [HttpPut("{id}/login")]
    public IActionResult Login(int id, [FromBody] LoginModel loginModel)
    {
      return NoContent();
    }
  }
}