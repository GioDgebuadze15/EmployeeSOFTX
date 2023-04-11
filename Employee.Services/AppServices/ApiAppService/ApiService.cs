using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Employee.Data.Forms;
using Employee.Services.AppServices.ApiAppService.ApiResponseAppService;
using Employee.Services.AppServices.HttpClientService;
using Employee.Services.AppServices.ParserService;
using Employee.Services.JsonConverters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Employee.Services.AppServices.ApiAppService;

public class ApiService : IApiService
{
    private readonly IHttpClientWrapper _iHttpClientWrapper;
    private readonly ILogger<ApiService> _logger;

    public ApiService(IHttpClientWrapper iHttpClientWrapper, ILogger<ApiService> logger)
    {
        _iHttpClientWrapper = iHttpClientWrapper;
        _logger = logger;
    }

    public async Task<List<Data.Models.Employee>> GetAllEmployees()
    {
        try
        {
            return await HandleApiCallErrors(async () =>
            {
                var response = await _iHttpClientWrapper.GetAsync("api/employee");
                return await response.Content.ReadFromJsonAsync<List<Data.Models.Employee>>(JsonSerializationOptions
                           .GetDefaultOptions())
                       ?? new List<Data.Models.Employee>();
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UpdateEmployeeForm> GetEmployee(int id)
    {
        try
        {
            return await HandleApiCallErrors(async () =>
            {
                var response = await _iHttpClientWrapper.GetAsync($"api/employee/{id}");
                return await response.Content.ReadFromJsonAsync<UpdateEmployeeForm>(JsonSerializationOptions
                           .GetDefaultOptions())
                       ?? new UpdateEmployeeForm();
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Data.Models.Employee>> GetEmployeesBySearchValue(string searchText)
    {
        try
        {
            return await HandleApiCallErrors(async () =>
            {
                var response = await _iHttpClientWrapper.GetAsync($"api/employee/search?searchString={searchText}");
                return await response.Content.ReadFromJsonAsync<List<Data.Models.Employee>>(JsonSerializationOptions
                           .GetDefaultOptions())
                       ?? new List<Data.Models.Employee>();
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponseEmployeeBase?> AddEmployee(CreateEmployeeForm createEmployeeForm,
        HttpContext httpContext)
    {
        try
        {
            var token = CheckToken(httpContext);
            return await HandleApiCallErrors(async () =>
            {
                var response =
                    await _iHttpClientWrapper.PostAsync("api/employee", GetStringContent(createEmployeeForm), token);
                return await response.Content.ReadFromJsonAsync<ApiResponseEmployeeBase>();
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponseEmployeeBase?> EditEmployee(UpdateEmployeeForm updateEmployeeForm,
        HttpContext httpContext)
    {
        try
        {
            var token = CheckToken(httpContext);
            return await HandleApiCallErrors(async () =>
            {
                var response =
                    await _iHttpClientWrapper.PutAsync("api/employee",
                        GetStringContent(updateEmployeeForm), token);
                return await response.Content.ReadFromJsonAsync<ApiResponseEmployeeBase>();
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponseEmployeeBase?> DeleteEmployee(int id,
        HttpContext httpContext)
    {
        try
        {
            var token = CheckToken(httpContext);
            return await HandleApiCallErrors(async () =>
            {
                var response =
                    await _iHttpClientWrapper.DeleteAsync($"api/employee/{id}", token);
                return await response.Content.ReadFromJsonAsync<ApiResponseEmployeeBase>();
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponseUserBase?> LoginUser(LoginUserForm loginUserForm)
    {
        try
        {
            return await HandleApiCallErrors(async () =>
            {
                var response =
                    await _iHttpClientWrapper.PostAsync("api/user/login", GetStringContent(loginUserForm), null);
                return await response.Content.ReadFromJsonAsync<ApiResponseUserBase>();
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponseUserBase?> RegisterUser(CreateUserForm createUserForm)
    {
        try
        {
            return await HandleApiCallErrors(async () =>
            {
                var response =
                    await _iHttpClientWrapper.PostAsync("api/user/register", GetStringContent(createUserForm), null);
                return await response.Content.ReadFromJsonAsync<ApiResponseUserBase>();
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<T> HandleApiCallErrors<T>(Func<Task<T>> function)
    {
        try
        {
            return await function();
        }
        catch (JsonException ex)
        {
            var errorMessage = $"Failed to parse JSON. Message: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            throw new Exception(errorMessage);
        }
        catch (HttpRequestException ex) when (ex.StatusCode != null)
        {
            var errorMessage = $"Failed to retrieve data from API. Status code: {ex.StatusCode}";
            _logger.LogError(ex, errorMessage);
            throw new Exception(errorMessage);
        }
        catch (HttpRequestException ex)
        {
            const string errorMessage = "Failed to retrieve data from API.";
            _logger.LogError(ex, errorMessage);
            throw new Exception(errorMessage);
        }
    }

    private static string CheckToken(HttpContext httpContext)
    {
        var cookieParser = new CookieParser(httpContext);
        if (!cookieParser.HasToken)
            throw new Exception("You are not allowed!");

        return cookieParser.Token!;
    }

    private static StringContent GetStringContent<T>(T data) =>
        new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
}