namespace SimpleRegistryTransfer.Entities.Codecs.WolfVariant;
public sealed record class WolfVariantCodec
{
    public required string Name { get; init; }

    public required int Id { get; init; }

    public required WolfVariantElement Element { get; init; }
}