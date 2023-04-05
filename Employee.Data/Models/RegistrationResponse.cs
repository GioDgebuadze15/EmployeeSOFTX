namespace Employee.Data.Models;

public record RegistrationResponse(int StatusCode, string? Error, string? Token);