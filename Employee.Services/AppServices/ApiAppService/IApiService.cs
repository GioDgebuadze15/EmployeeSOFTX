using System.Text.Json;
using Employee.Services.AppServices.ApiAppService.ApiResponseAppService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Services.AppServices.ApiAppService;

public interface IApiService
{
    Task<IActionResult> HandleApiGetCall<TRequest, TResponse>(string apiPath, JsonSerializerOptions? options,
        string redirectPageName)
        where TRequest : class
        where TResponse : class;

    Task<IActionResult> HandleApiCall<TRequest, TResponse>(
        HttpMethod httpMethod,
        string apiPath,
        TRequest? requestData,
        JsonSerializerOptions? options,
        string redirectAction,
        HttpContext? httpContext)
        where TRequest : class
        where TResponse : IApiResponse;
}