namespace SimpleRegistryTransfer.Entities.Codecs;
public sealed class BiomeVariantCodec
{
    public required string Name { get; init; }

    public required int Id { get; init; }

    public required BiomeVariantElement Element { get; set; }
}