using SimpleRegistryTransfer.Entities.Codecs.Dimension;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleRegistryTransfer.Converters;
public sealed class MonsterLightLevelConverter : JsonConverter<MonsterSpawnLightLevel>
{
    public override MonsterSpawnLightLevel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var doc = JsonDocument.ParseValue(ref reader);

        var element = doc.RootElement;

        if (element.ValueKind == JsonValueKind.Object)
            return new() { Value = element.Deserialize<MonsterSpawnLightLevelValue>(Helpers.CodecJsonOptions) };

        return new() { IntValue = doc.RootElement.GetInt32() };
    }

    public override void Write(Utf8JsonWriter writer, MonsterSpawnLightLevel value, JsonSerializerOptions options)
    {
        if (value.Value.HasValue)
        {
            var msLevel = value.Value.Value;
            writer.WriteStartObject();

            writer.WriteString("type", msLevel.Type);
            writer.WriteNumber("max_inclusive", msLevel.MaxInclusive);
            writer.WriteNumber("min_inclusive", msLevel.MinInclusive);

            writer.WriteEndObject();
            return;
        }

        writer.WriteNumberValue(value.IntValue.Value);
    }
}
