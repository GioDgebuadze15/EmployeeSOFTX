namespace Employee.Data.Models;

public record DeleteEmployeeResponse(int StatusCode, string? Error, object? Data);