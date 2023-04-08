using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace Employee.Services.AppServices.HttpClientService;

public static class HttpClientFactory
{
    public static HttpClient Create(IConfiguration configuration)
    {
        var baseUrl = configuration.GetValue<string>("ApiBaseUrl");
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(baseUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return httpClient;
    }
}