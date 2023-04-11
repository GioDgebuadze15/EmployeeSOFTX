using System.Diagnostics;
using Employee.Data.Forms;
using Employee.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Employee.Services.AppServices.ApiAppService;
using Microsoft.AspNetCore.Authorization;

namespace Employee.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IApiService _iApiService;

    public HomeController(IApiService iApiService)
    {
        _iApiService = iApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            return View(await _iApiService.GetAllEmployees());
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel {Message = ex.Message});
        }
    }

    [HttpGet]
    public async Task<IActionResult> FilterEmployees([FromQuery] string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
            return RedirectToAction("Index");
        try
        {
            var result = await _iApiService.GetEmployeesBySearchValue(searchText);
            return PartialView("_EmployeeTable", result);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel {Message = ex.Message});
        }
    }


    [HttpGet]
    [Authorize]
    public IActionResult AddEmployee()
        =>
            View(new CreateEmployeeForm());


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddEmployee(CreateEmployeeForm createEmployeeForm)
    {
        if (!ModelState.IsValid)
            return View(createEmployeeForm);

        try
        {
            var result = await _iApiService.AddEmployee(createEmployeeForm, HttpContext);
            switch (result?.StatusCode)
            {
                case 400:
                    TempData["ErrorMessage"] = result.Error;
                    return View(createEmployeeForm);
                case 200:
                    return RedirectToAction("Index");
                default:
                    return View("Error", new ErrorViewModel {Message = result?.Error ?? "An error occured!"});
            }
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel {Message = ex.Message});
        }
    }

    [HttpGet("EditEmployee/{id::int}")]
    [Authorize]
    public async Task<IActionResult> EditEmployee(int id)
    {
        if (id < 1)
            return View("Error",
                new ErrorViewModel {Message = "An error occured!"});

        try
        {
            return View(await _iApiService.GetEmployee(id));
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel {Message = ex.Message});
        }
    }

    [HttpPost("EditEmployee/{id:int}")]
    [Authorize]
    public async Task<IActionResult> EditEmployee(int id, UpdateEmployeeForm updateEmployeeForm)
    {
        if (id < 1)
            return View("Error",
                new ErrorViewModel {Message = "An error occured!"});

        if (!ModelState.IsValid)
            return View(updateEmployeeForm);

        try
        {
            var result = await _iApiService.EditEmployee(updateEmployeeForm, HttpContext);
            switch (result?.StatusCode)
            {
                case 400:
                    TempData["ErrorMessage"] = result.Error;
                    return View(updateEmployeeForm);
                case 200:
                    return RedirectToAction("Index");
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
    [Authorize]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        if (id < 1)
            return View("Error",
                new ErrorViewModel {Message = "An error occured!"});
        try
        {
            var result = await _iApiService.DeleteEmployee(id, HttpContext);
            return result?.StatusCode switch
            {
                200 => RedirectToAction("Index"),
                _ => View("Error", new ErrorViewModel {Message = result?.Error ?? "An error occured!"})
            };
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel {Message = ex.Message});
        }
    }


    public IActionResult Privacy()
        => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorMessage)
        => View(new ErrorViewModel
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = errorMessage});
}