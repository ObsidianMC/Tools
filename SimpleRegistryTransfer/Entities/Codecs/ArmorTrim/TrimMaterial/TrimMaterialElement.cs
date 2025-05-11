using System.Collections.Generic;

namespace SimpleRegistryTransfer.Entities.Codecs.ArmorTrim.TrimMaterial;
public sealed class TrimMaterialElement
{
    public required string AssetName { get; init; }

    public required ArmorTrimDescription Description { get; init; }

    public Dictionary<string, string> OverrideArmorAssets { get; init; }
}
