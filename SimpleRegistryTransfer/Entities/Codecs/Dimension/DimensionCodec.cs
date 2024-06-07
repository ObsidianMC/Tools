namespace SimpleRegistryTransfer.Entities.Codecs.Dimension;
public sealed class DimensionCodec
{
    public string Name { get; set; }

    public int Id { get; set; }

    public DimensionElement Element { get; set; }
}
