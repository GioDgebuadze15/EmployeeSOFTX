using System.Diagnostics;
using Employee.Data.Forms;
using Employee.Data.Models;
using Employee.Services.AppServices.ApiAppService;
using Employee.Services.AppServices.ApiAppService.ApiResponseAppService;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IApiService _iApiService;

    public AccountController(ILogger<AccountController> logger, IApiService iApiService)
    {
        _logger = logger;
        _iApiService = iApiService;
    }


    [HttpGet]
    public IActionResult SignIn()
        => View(new LoginUserForm());

    [HttpPost]
    public async Task<IActionResult> SignIn(LoginUserForm loginUserForm)
    {
        if (!ModelState.IsValid)
            return View("SignIn", loginUserForm);

        var result = await _iApiService.HandleApiCall<object, ApiResponseUserBase>
        (HttpMethod.Post,
            "api/user/login",
            loginUserForm,
            null,
            "Index",
            null
        );
        
        var token = (result as ContentResult)?.Content;
        if (token is null) return result;

        HttpContext.Response.Cookies.Append("employee-token", token);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult LogOut()
    {
        HttpContext.Response.Cookies.Delete("employee-token");
        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public IActionResult SignUp()
    {
        return View(new CreateUserForm());
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(CreateUserForm createUserForm)
    {
        if (!ModelState.IsValid)
            return View("SignUp", createUserForm);

        var result = await _iApiService.HandleApiCall<object, ApiResponseUserBase>
        (HttpMethod.Post,
            "api/user/register",
            createUserForm,
            null,
            "Index",
            null
        );


        var token = (result as ContentResult)?.Content;
        if (token is null) return result;

        HttpContext.Response.Cookies.Append("employee-token", token);
        return RedirectToAction("Index", "Home");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorMessage)
    {
        return View(new ErrorViewModel
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = errorMessage});
    }
}