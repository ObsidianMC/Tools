namespace SimpleRegistryTransfer.Entities.Codecs.ArmorTrim.TrimPattern;
public sealed class TrimPatternElement
{
    public required string AssetId { get; init; }

    public required bool Decal { get; init; }

    public required ArmorTrimDescription Description { get; init; }
}
