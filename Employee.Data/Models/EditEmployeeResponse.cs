namespace Employee.Data.Models;

public record EditEmployeeResponse(int StatusCode, string? Error, object? Data);