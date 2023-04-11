using System.Diagnostics;
using Employee.Data.Forms;
using Employee.Data.Models;
using Employee.Services.AppServices.ApiAppService;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly IApiService _iApiService;

    public AccountController(IApiService iApiService)
    {
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
        try
        {
            var result = await _iApiService.LoginUser(loginUserForm);
            switch (result?.StatusCode)
            {
                case 404:
                    TempData["ErrorMessage"] = result.Error;
                    return View(loginUserForm);
                case 200:
                    HttpContext.Response.Cookies.Append("employee-token", result.Token!);
                    return RedirectToAction("Index", "Home");
                default:
                    return View("Error", new ErrorViewModel {Message = result?.Error ?? "An error occured!"});
            }
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel {Message = ex.Message});
        }
    }

    [HttpPost]
    public IActionResult LogOut()
    {
        HttpContext.Response.Cookies.Delete("employee-token");
        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public IActionResult SignUp()
        => View(new CreateUserForm());

    [HttpPost]
    public async Task<IActionResult> SignUp(CreateUserForm createUserForm)
    {
        if (!ModelState.IsValid)
            return View(createUserForm);

        try
        {
            var result = await _iApiService.RegisterUser(createUserForm);
            switch (result?.StatusCode)
            {
                case 400:
                    TempData["ErrorMessage"] = result.Error;
                    return View(createUserForm);
                case 200:
                    HttpContext.Response.Cookies.Append("employee-token", result.Token!);
                    return RedirectToAction("Index", "Home");
                default:
                    return View("Error", new ErrorViewModel {Message = result?.Error ?? "An error occured!"});
            }
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel {Message = ex.Message});
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorMessage)
        => View(new ErrorViewModel
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = errorMessage});
}