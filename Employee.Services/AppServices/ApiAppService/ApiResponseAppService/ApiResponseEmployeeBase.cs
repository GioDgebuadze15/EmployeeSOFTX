namespace Employee.Services.AppServices.ApiAppService.ApiResponseAppService;

public record ApiResponseEmployeeBase(int StatusCode, string? Error, object? Data) : IApiResponse;