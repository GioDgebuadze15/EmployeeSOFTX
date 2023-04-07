using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Employee.Mvc.Models;
using Newtonsoft.Json;

namespace Employee.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
    {
        // Call the API endpoint to retrieve data
        var response = await _httpClient.GetAsync("https://localhost:7219/api/employee");

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            // Convert the response content to a string
            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into your model class
            var model = JsonConvert.DeserializeObject<List<test>>(responseBody);

            // Pass the model to your view
            return View(model);
        }

        // else
        // {
        //     // Handle errors
        //     string errorMessage = $"Failed to retrieve data from API. Status code: {response.StatusCode}";
        //     return View("Error", new ErrorViewModel { Message = errorMessage });
        // }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:7219/api/employee/{id}");
        if (response.IsSuccessStatusCode)
        {
            // Convert the response content to a string
            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into your model class
            var model = JsonConvert.DeserializeObject<List<test>>(responseBody);

            // Pass the model to your view
        }

        return RedirectToAction("Index");
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

public class test
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public ulong PersonalId { get; set; }

    public string? Gender { get; set; }

    public string? MobileNumber { get; set; }

    public string Position { get; set; }

    public string EmployeeStatus { get; set; }

    public DateTime? DateOfFire { get; set; }

    public DateTime CreatedDate { get; set; }
}