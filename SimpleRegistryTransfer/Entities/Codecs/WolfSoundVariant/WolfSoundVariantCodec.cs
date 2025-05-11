namespace SimpleRegistryTransfer.Entities.Codecs.WolfSoundVariant;
public sealed record class WolfSoundVariantCodec
{
    public required string Name { get; init; }

    public required int Id { get; init; }

    public required WolfSoundVariantElement Element { get; init; }
}