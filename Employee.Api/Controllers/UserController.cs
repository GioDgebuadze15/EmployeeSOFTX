using Employee.Data.Forms;
using Employee.Services.AppServices.UserAppService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Api.Controllers;

[Route("api/user")]
[AllowAnonymous]
public class UserController : ApiController
{
    private readonly IUserService _iUserService;

    public UserController(IUserService iUserService)
    {
        _iUserService = iUserService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserForm createUserForm)
        => Ok(await _iUserService.RegisterUser(createUserForm));

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginUserForm loginUserForm)
        => Ok(_iUserService.LoginUser(loginUserForm));
}