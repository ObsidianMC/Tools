namespace SimpleRegistryTransfer.Entities.Codecs.PaintingVariant;
public sealed class PaintingVariantCodec
{
    public required string Name { get; init; }

    public required int Id { get; init; }

    public PaintingVariantElement Element { get; set; }
}
