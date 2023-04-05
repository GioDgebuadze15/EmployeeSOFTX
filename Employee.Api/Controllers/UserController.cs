using Employee.Data.Forms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Api.Controllers;

[Route("api/user")]
[AllowAnonymous]
public class UserController : ApiController
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] CreateUserForm createUserForm)
    {
        return Ok();
    }
}