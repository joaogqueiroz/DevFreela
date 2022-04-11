using Microsoft.AspNetCore.Mvc;
using DevFreela.Api.Models;
using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
namespace DevFreela.Api.Controllers
{
  [Route("api/users")]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
      _userService = userService;
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
    public IActionResult Post([FromBody] CreateUserInputModel inputModel)
    {
      var id = _userService.Create(inputModel);

      return CreatedAtAction(nameof(GetById), new { id = id }, inputModel);
    }
    // api/users/1/login   
    [HttpPut("{id}/login")]
    public IActionResult Login(int id, [FromBody] LoginModel loginModel)
    {
      return NoContent();
    }
  }
}