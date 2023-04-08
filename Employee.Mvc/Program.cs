using Employee.Data.Forms.Validation;
using Employee.Services.AppServices;
using Employee.Services.AppServices.HttpClientService;
using Employee.Services.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
// builder.Services.AddSingleton<ITokenStore, InMemoryTokenStore>();

builder.Services.AddAuthentication("jwt")
    .AddJwtBearer("jwt", o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
        };
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                if (ctx.Request.Query.ContainsKey("t"))
                {
                    ctx.Token = ctx.Request.Query["t"];
                }

                return Task.CompletedTask;
            }
        };
        o.Configuration = new OpenIdConnectConfiguration
        {
            SigningKeys =
            {
                new RsaSecurityKey(RsaKey.GetRsaKey())
            }
        };
        o.MapInboundClaims = false;
    });

// Automatic Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(CreateUserFormValidation).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LoginUserFormValidation).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateEmployeeValidation).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(UpdateEmployeeValidation).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// app.UseMiddleware<TokenAuthMiddleware>();

app.UseRouting();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();