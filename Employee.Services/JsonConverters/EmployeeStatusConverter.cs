using System.Text.Json;
using System.Text.Json.Serialization;
using Employee.Data.Models;

namespace Employee.Services.JsonConverters;

public class EmployeeStatusConverter : JsonConverter<EmployeeStatus>
{
    public override EmployeeStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var employeeStatusStr = reader.GetString();
        return employeeStatusStr switch
        {
            "In State" => EmployeeStatus.InState,
            "Out Of State" => EmployeeStatus.OutOfState,
            "Fired" => EmployeeStatus.Fired,
            _ => throw new JsonException($"Invalid employee status value: {employeeStatusStr}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, EmployeeStatus value, JsonSerializerOptions options)
    {
    }
}