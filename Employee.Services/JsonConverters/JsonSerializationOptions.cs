using System.Text.Json;

namespace Employee.Services.JsonConverters;

public static class JsonSerializationOptions
{
    public static JsonSerializerOptions GetDefaultOptions()
        => new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = {new GenderConverter(), new EmployeeStatusConverter()}
        };

    public static JsonSerializerOptions GetEmployeeOptions()
        => new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = {new EmployeeStatusConverter()}
        };
}