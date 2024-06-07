namespace SimpleRegistryTransfer.Entities.Codecs.ArmorTrim.TrimPattern;
public sealed class TrimPatternCodec
{
    public required string Name { get; init; }

    public required int Id { get; init; }

    public required TrimPatternElement Element { get; init; }
}
