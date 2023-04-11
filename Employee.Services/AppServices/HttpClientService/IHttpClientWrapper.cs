namespace Employee.Services.AppServices.HttpClientService;

public interface IHttpClientWrapper
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    Task<HttpResponseMessage> GetAsync(string endpoint);
    Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content, string? token);
    Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content, string token);
    Task<HttpResponseMessage> DeleteAsync(string endpoint, string token);
}