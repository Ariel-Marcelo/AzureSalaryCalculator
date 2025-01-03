using Newtonsoft.Json;

namespace SalaryCalculator.App.Shared.Utils;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(DateFormat));
    }

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String && DateOnly.TryParseExact((string)reader.Value!, DateFormat, out var date))
        {
            return date;
        }

        throw new JsonSerializationException($"Invalid DateOnly format. Expected format: {DateFormat}");
    }
}
