namespace Employee.Services.AppServices.ApiAppService.ApiResponseAppService;

public record ApiResponseUserBase(int StatusCode, string? Error, string? Token): IApiResponse;