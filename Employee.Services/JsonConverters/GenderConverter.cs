using System.Text.Json;
using System.Text.Json.Serialization;
using Employee.Data.Models;

namespace Employee.Services.JsonConverters;

public class GenderConverter : JsonConverter<Gender>
{
    public override Gender Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var genderStr = reader.GetString();
        return genderStr switch
        {
            "Male" => Gender.Male,
            "Female" => Gender.Female,
            _ => throw new JsonException($"Invalid employee status value: {genderStr}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, Gender value, JsonSerializerOptions options)
    {
    }
}