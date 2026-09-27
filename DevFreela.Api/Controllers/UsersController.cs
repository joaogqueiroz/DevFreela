using Microsoft.AspNetCore.Mvc;
using DevFreela.Application.Commands.CreateUser;
using DevFreela.Application.Commands.LoginUser;
using MediatR;
using System.Threading.Tasks;
using System.Linq;
using DevFreela.Application.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
namespace DevFreela.Api.Controllers
{
  [Route("api/users")]
  [Authorize]
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
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
    {
      var id = await _mediator.Send(command);
      return CreatedAtAction(nameof(GetById), new { id = id }, command);
    }
    // api/users/login   
    [HttpPut("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
      var loginUserViewModel = await _mediator.Send(command);

      if (loginUserViewModel == null)
      {
        return BadRequest();
      }

      return Ok(loginUserViewModel);
    }
  }
}