namespace SimpleRegistryTransfer.Entities.Codecs.ArmorTrim.TrimMaterial;
public sealed class TrimMaterialCodec
{
    public required string Name { get; init; }

    public required int Id { get; init; }

    public required TrimMaterialElement Element { get; init; }
}
