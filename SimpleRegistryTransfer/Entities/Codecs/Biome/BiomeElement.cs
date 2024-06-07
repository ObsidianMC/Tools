using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleRegistryTransfer.Entities.Codecs.Biome;

public class BiomeElement
{
    public BiomeEffect Effects { get; set; }

    public float Depth { get; set; }

    public float Temperature { get; set; }

    public float Scale { get; set; }

    public float Downfall { get; set; }

    public string Category { get; set; }

    public bool HasPrecipitation { get; set; }

    public string TemperatureModifier { get; set; }//TODO turn into enum

    public bool PlayerSpawnFriendly { get; set; }

    public string[][] Features { get; set; } = [];

    [JsonConverter(typeof(SpecialDictionaryConverter))]
    public Dictionary<string, string[]> Carvers { get; set; }
    public Dictionary<string, SpawnerMob[]> Spawners { get; set; }
    public Dictionary<string, object> SpawnCosts { get; set; }
}
public sealed class SpawnerMob
{
    public required string Type { get; init; }

    [JsonPropertyName("maxCount")]
    public required int MaxCount { get; init; }

    [JsonPropertyName("minCount")]
    public required int MinCount { get; init; }

    [JsonPropertyName("weight")]
    public required int Weight { get; init; }
}

