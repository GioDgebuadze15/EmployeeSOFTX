using System.Text;
using Employee.Data.Forms;
using Employee.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Employee.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly HttpClient _httpClient;

    public AccountController(ILogger<AccountController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }


    // GET
    public IActionResult SignIn()
    {
        return View(new LoginUserForm());
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(LoginUserForm loginUserForm)
    {
        if (!ModelState.IsValid)
            return View();

        var json = JsonConvert.SerializeObject(loginUserForm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Call the API endpoint to retrieve data
        var response = await _httpClient.PostAsync("https://localhost:7219/api/user/login", content);

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            // Convert the response content to a string
            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into your model class
            var model = JsonConvert.DeserializeObject<LoginResponse>(responseBody);

            // Pass the model to your view
            if (model is {Error: { }})
            {
                ModelState.AddModelError("All", model.Error);
            }

            // set token cookie
            HttpContext.Response.Cookies.Append("employee-token",model.Token);
            return RedirectToAction("Index", "Home");
        }

        return View();
    }


    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignUp(CreateUserForm createUserForm)
    {
        return View();
    }
}