using Microsoft.AspNetCore.Http;

namespace Employee.Services.AppServices.ParserService;

public class CookieParser
{
    private readonly HttpContext _httpContext;
    public string? Token => GetToken();
    public bool HasToken => Token != null && !string.IsNullOrEmpty(Token);

    public CookieParser(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    private string? GetToken()
        =>
            _httpContext.Request.Cookies.TryGetValue("employee-token", out var employeeToken)
                ? employeeToken
                : null;
}