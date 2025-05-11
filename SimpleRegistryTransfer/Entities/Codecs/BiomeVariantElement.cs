using System.Collections.Generic;

namespace SimpleRegistryTransfer.Entities.Codecs;
public sealed record class BiomeVariantElement
{
    public required string AssetId { get; set; }

    public string? Model { get; set; }

    public List<SpawnConditionElement> SpawnConditions { get; set; }
}