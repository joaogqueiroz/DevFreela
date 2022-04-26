using Microsoft.AspNetCore.Mvc;
using DevFreela.Api.Models;
using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.Commands.CreateUser;
using MediatR;
using System.Threading.Tasks;
namespace DevFreela.Api.Controllers
{
  [Route("api/users")]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly IMediator _mediator;
    public UsersController(IUserService userService, IMediator mediator)
    {
      _userService = userService;
      _mediator = mediator;
    }
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
      var user = _userService.GetById(id);

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