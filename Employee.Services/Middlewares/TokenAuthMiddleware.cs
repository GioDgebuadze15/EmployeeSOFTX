using Microsoft.AspNetCore.Http;

namespace Employee.Services.Middlewares;

public class TokenAuthMiddleware
{
    private readonly RequestDelegate _next;

    public TokenAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenStore tokenStore)
    {
        var token = context.Request.Cookies["Token"];

        // check if token is valid
        if (string.IsNullOrEmpty(token) || !tokenStore.IsValidToken(token))
        {
            context.Response.StatusCode = 401; // Unauthorized
            return;
        }

        // token is valid, continue with the next middleware
        await _next(context);
    }
}

public interface ITokenStore
{
    bool IsValidToken(string token);
}

public class InMemoryTokenStore : ITokenStore
{
    private readonly IDictionary<string, string> _tokens = new Dictionary<string, string>();

    public void StoreToken(string username, string token)
    {
        _tokens[username] = token;
    }

    public bool IsValidToken(string token)
    {
        return _tokens.Values.Contains(token);
    }
}