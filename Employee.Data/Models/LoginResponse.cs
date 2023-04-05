namespace Employee.Data.Models;

public record LoginResponse(int StatusCode, string? Error, string? Token);