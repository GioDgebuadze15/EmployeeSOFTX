using System.Diagnostics;
using System.Text.Json;
using Employee.Data.Forms;
using Employee.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Employee.Mvc.Models;
using Employee.Services.AppServices.HttpClientService;
using Employee.Services.AppServices.ParserService;
using Employee.Services.JsonConverters;
using Microsoft.AspNetCore.Authorization;

namespace Employee.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientWrapper _iHttpClientWrapper;

    public HomeController(ILogger<HomeController> logger, IHttpClientWrapper iHttpClientWrapper)
    {
        _logger = logger;
        _iHttpClientWrapper = iHttpClientWrapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new GenderConverter(),
                    new EmployeeStatusConverter()
                }
            };
            using var response = await _iHttpClientWrapper.GetAsync("api/employee");

            var result = await response.Content.ReadFromJsonAsync<List<Data.Models.Employee>>(options);
            return View(result);
        }
        catch (JsonException ex)
        {
            //Todo: log the exception into file 
            var errorMessage = $"Failed to parse json. Message: {ex.Message}";
            return View("Error", new ErrorViewModel {Message = errorMessage});
        }
        catch (HttpRequestException ex)
        {
            //Todo: log the exception into file 
            var errorMessage = $"Failed to retrieve data from API. Status code: {ex.StatusCode}";
            return View("Error", new ErrorViewModel {Message = errorMessage});
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult AddEmployee()
    {
        return View(new CreateEmployeeForm());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddEmployee(CreateEmployeeForm createEmployeeForm)
    {
        if (!ModelState.IsValid)
            return View("AddEmployee", createEmployeeForm);

        var cookieParser = new CookieParser(HttpContext);
        if (!cookieParser.HasToken)
            return View("Error",
                new ErrorViewModel {Message = "You are not allowed!"});
        try
        {
            using var response =
                await _iHttpClientWrapper.PostAsync("api/employee", new StringContent(""), cookieParser.Token!);

            var result = await response.Content.ReadFromJsonAsync<AddEmployeeResponse>();
            return result?.StatusCode switch
            {
                200 => RedirectToAction("Index"),
                _ => View("Error",
                    new ErrorViewModel {Message = result?.Error ?? "An error occured while adding employee!"})
            };
        }
        catch (JsonException ex)
        {
            //Todo: log the exception into file 
            var errorMessage = $"Failed to parse json. Message: {ex.Message}";
            return View("Error", new ErrorViewModel {Message = errorMessage});
        }
        catch (HttpRequestException ex)
        {
            //Todo: log the exception into file 
            var errorMessage = $"Failed to retrieve data from API. Status code: {ex.StatusCode}";
            return View("Error", new ErrorViewModel {Message = errorMessage});
        }
    }

    [HttpGet("EditEmployee/{id::int}")]
    [Authorize]
    public async Task<IActionResult> EditEmployee(int id)
    {
        try
        {
            using var response = await _iHttpClientWrapper.GetAsync($"api/employee/{id}");

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new EmployeeStatusConverter()
                }
            };
            var result = await response.Content.ReadFromJsonAsync<UpdateEmployeeForm>(options);
            return View(result);
        }
        catch (JsonException ex)
        {
            //Todo: log the exception into file 
            var errorMessage = $"Failed to parse json. Message: {ex.Message}";
            return View("Error", new ErrorViewModel {Message = errorMessage});
        }
        catch (HttpRequestException ex)
        {
            //Todo: log the exception into file 
            var errorMessage = $"Failed to retrieve data from API. Message: {ex.Message}";
            return View("Error", new ErrorViewModel {Message = errorMessage});
        }
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var cookieParser = new CookieParser(HttpContext);
        if (!cookieParser.HasToken)
            return View("Error",
                new ErrorViewModel {Message = "You are not allowed!"});
        try
        {
            using var response = await _iHttpClientWrapper.DeleteAsync($"api/employee/{id}", cookieParser.Token!);
            var result = await response.Content.ReadFromJsonAsync<DeleteEmployeeResponse>();
            return result?.StatusCode switch
            {
                200 => RedirectToAction("Index"),
                _ => View("Error",
                    new ErrorViewModel {Message = result?.Error ?? "An error occured while processing deletion!"})
            };
        }
        catch (JsonException ex)
        {
            //Todo: log the exception into file 
            var errorMessage = $"Failed to parse json. Message: {ex.Message}";
            return View("Error", new ErrorViewModel {Message = errorMessage});
        }
        catch (HttpRequestException ex)
        {
            //Todo: log the exception into file 
            var errorMessage = $"Failed to retrieve data from API. Message: {ex.Message}";
            return View("Error", new ErrorViewModel {Message = errorMessage});
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}