using System.Net.Http.Headers;
using Employee.Services.AppServices.ParserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Employee.Services.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var cookieParser = new CookieParser(context);
        var endpoint = context.GetEndpoint();
        if (endpoint != null && endpoint.Metadata.GetMetadata<AuthorizeAttribute>() == null)
        {
            await _next(context);
            return;
        }

        if (!cookieParser.HasToken)
        {
            context.Response.Redirect("/Account/SignIn");
            return;
        }

        context.Request.Headers.Add("Authorization",
            new AuthenticationHeaderValue("Bearer", cookieParser.Token).ToString());

        await _next(context);
    }
}