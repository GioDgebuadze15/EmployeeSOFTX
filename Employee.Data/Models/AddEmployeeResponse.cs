namespace Employee.Data.Models;

public record AddEmployeeResponse(int StatusCode, string? Error, object? Data);