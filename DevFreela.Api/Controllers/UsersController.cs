using Microsoft.AspNetCore.Mvc;
using DevFreela.Api.Models;
namespace DevFreela.Api.Controllers
{
  [Route("api/users")]
  public class UsersController : ControllerBase
  {
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
      return Ok();
    }
    [HttpPost]
    public IActionResult Post([FromBody] CreateUserModel createUserModel)
    {
      return CreatedAtAction(nameof(GetById), new { id = 1 }, createUserModel);
    }
    // api/users/1/login   
    [HttpPut("{id}/login")]
    public IActionResult Login(int id, [FromBody] LoginModel loginModel)
    {
      return NoContent();
    }
  }
}