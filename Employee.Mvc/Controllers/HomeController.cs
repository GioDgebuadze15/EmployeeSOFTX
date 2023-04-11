using System.Diagnostics;
using Employee.Data.Forms;
using Employee.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Employee.Services.AppServices.ApiAppService;
using Employee.Services.AppServices.ApiAppService.ApiResponseAppService;
using Employee.Services.JsonConverters;
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
        => await _iApiService.HandleApiGetCall<object, List<Data.Models.Employee>>
        ("api/employee",
            JsonSerializationOptions.GetDefaultOptions(),
            "Index");


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
            return View("AddEmployee", createEmployeeForm);

        return await _iApiService.HandleApiCall<object, ApiResponseEmployeeBase>
        (HttpMethod.Post,
            "api/employee",
            createEmployeeForm,
            null,
            "Index", HttpContext);
    }

    [HttpGet("EditEmployee/{id::int}")]
    [Authorize]
    public async Task<IActionResult> EditEmployee(int id)
    {
        if (id < 1)
            return View("Error",
                new ErrorViewModel {Message = "An error occured!"});

        return await _iApiService.HandleApiGetCall<object, Data.Models.Employee>
        ($"api/employee/{id}",
            JsonSerializationOptions.GetEmployeeOptions(),
            "EditEmployee");
    }

    [HttpPost("EditEmployee/{id:int}")]
    [Authorize]
    public async Task<IActionResult> EditEmployee(int id, UpdateEmployeeForm updateEmployeeForm)
    {
        if (id < 1)
            return View("Error",
                new ErrorViewModel {Message = "An error occured!"});

        if (!ModelState.IsValid)
            return View("EditEmployee", updateEmployeeForm);

        return await _iApiService.HandleApiCall<object, ApiResponseEmployeeBase>
        (HttpMethod.Put,
            "api/employee",
            updateEmployeeForm,
            null,
            "Index",
            HttpContext);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteEmployee(int id)
        =>
            await _iApiService.HandleApiCall<object, ApiResponseEmployeeBase>
            (HttpMethod.Delete,
                $"api/employee/{id}",
                null,
                null,
                "Index",
                HttpContext);


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorMessage)
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,Message = errorMessage});
    }
}