namespace SimpleRegistryTransfer.Entities.Codecs.ArmorTrim.TrimMaterial;
public sealed class TrimMaterialElement
{
    public required string AssetName { get; init; }

    public required ArmorTrimDescription Description { get; init; }

    public required string Ingredient { get; init; }

    public required double ItemModelIndex { get; init; }
}
