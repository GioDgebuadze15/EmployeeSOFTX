using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Employee.Data.Models;
using Employee.Services.AppServices.ApiAppService.ApiResponseAppService;
using Employee.Services.AppServices.HttpClientService;
using Employee.Services.AppServices.ParserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

    public async Task<IActionResult> HandleApiGetCall<TRequest, TResponse>(
        string apiPath,
        JsonSerializerOptions? options,
        string redirectPageName)
        where TRequest : class
        where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Get, apiPath);
        try
        {
            var response = await _iHttpClientWrapper.SendAsync(request);

            var result = await response.Content.ReadFromJsonAsync<TResponse>(options);

            return PageView(redirectPageName, result);
        }
        catch (JsonException ex)
        {
            var errorMessage = $"Failed to parse JSON. Message: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return ErrorView(errorMessage);
        }
        catch (HttpRequestException ex) when (ex.StatusCode != null)
        {
            var errorMessage = $"Failed to retrieve data from API. Status code: {ex.StatusCode}";
            _logger.LogError(ex, errorMessage);
            return ErrorView(errorMessage);
        }
        catch (HttpRequestException ex)
        {
            const string errorMessage = "Failed to retrieve data from API.";
            _logger.LogError(ex, errorMessage);
            return ErrorView(errorMessage);
        }
        finally
        {
            request.Dispose();
        }
    }

    public async Task<IActionResult> HandleApiCall<TRequest, TResponse>(
        HttpMethod httpMethod,
        string apiPath,
        TRequest? requestData,
        JsonSerializerOptions? options,
        string redirectAction,
        HttpContext? httpContext)
        where TRequest : class
        where TResponse : IApiResponse
    {
        var request = new HttpRequestMessage(httpMethod, apiPath);

        if (httpContext is not null)
        {
            var token = CheckToken(httpContext);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }


        try
        {
            if (requestData is not null)
                request.Content = GetStringContent(requestData);

            var response = await _iHttpClientWrapper.SendAsync(request);

            var result = await response.Content.ReadFromJsonAsync<TResponse>(options);

            if (result?.GetType() == typeof(ApiResponseEmployeeBase))
            {
                var employeeResponse = result as ApiResponseEmployeeBase;
                return employeeResponse?.StatusCode switch
                {
                    200 => PageView(redirectAction, employeeResponse),
                    _ => ErrorView(employeeResponse?.Error ?? "An error occurred while processing your request.")
                };
            }

            if (result?.GetType() == typeof(ApiResponseUserBase))
            {
                var userResponse = result as ApiResponseUserBase;

                return userResponse?.StatusCode switch
                {
                    200 => new ContentResult
                    {
                        Content = userResponse.Token,
                        ContentType = "text/plain",
                        StatusCode = 200
                    },
                    _ => ErrorView(userResponse?.Error ?? "An error occurred while processing your request.")
                };
            }

            _logger.LogError("Response from Api is not correct type");
            return ErrorView("An error occurred! Please try again.");
        }
        catch (JsonException ex)
        {
            var errorMessage = $"Failed to parse JSON. Message: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return ErrorView(errorMessage);
        }
        catch (HttpRequestException ex) when (ex.StatusCode != null)
        {
            var errorMessage = $"Failed to retrieve data from API. Status code: {ex.StatusCode}";
            _logger.LogError(ex, errorMessage);
            return ErrorView(errorMessage);
        }
        catch (HttpRequestException ex)
        {
            const string errorMessage = "Failed to retrieve data from API.";
            _logger.LogError(ex, errorMessage);
            return ErrorView(errorMessage);
        }
        finally
        {
            request.Dispose();
        }
    }

    private static string CheckToken(HttpContext httpContext)
    {
        var cookieParser = new CookieParser(httpContext);
        if (!cookieParser.HasToken)
        {
            ErrorView("You are not allowed!");
        }

        return cookieParser.Token!;
    }

    private static StringContent GetStringContent<T>(T data) =>
        new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

    private static IActionResult PageView<TModel>(string redirectPageName, TModel? model) where TModel : class?
    {
        return new ViewResult
        {
            ViewName = redirectPageName,
            ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = model,
            },
        };
    }

    private static IActionResult ErrorView(string errorMessage)
    {
        return new ViewResult
        {
            ViewName = "Error",
            ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = new ErrorViewModel {Message = errorMessage}
            }
        };
    }
}