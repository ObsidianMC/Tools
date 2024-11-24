using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleRegistryTransfer;
public sealed class SpecialArrayConverter : JsonConverter<string[]>
{
    public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var doc = JsonDocument.ParseValue(ref reader);

        var list = new List<string>();

        if (doc.RootElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var prop in doc.RootElement.EnumerateArray())
            {
                list.Add(prop.GetString());
            }
        }
        else
        {
            list.Add(doc.RootElement.GetString());
        }

        return [.. list];
    }

    public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
    {
        if (value.Length == 1)
        {
            writer.WriteStringValue(value[0]);
            return;
        }

        writer.WriteStartArray();
        foreach (var val in value)
        {
            writer.WriteStringValue(val);
        }
        writer.WriteEndArray();
    }
}

