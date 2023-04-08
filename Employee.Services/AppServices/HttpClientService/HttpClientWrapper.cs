using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace Employee.Services.AppServices.HttpClientService;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(IConfiguration configuration)
    {
        _httpClient = HttpClientFactory.Create(configuration);
    }


    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        return await _httpClient.GetAsync(endpoint);
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _httpClient.PostAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _httpClient.PutAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _httpClient.DeleteAsync(endpoint);
    }
}