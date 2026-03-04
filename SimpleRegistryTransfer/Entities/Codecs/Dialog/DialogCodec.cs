namespace SimpleRegistryTransfer.Entities.Codecs.Dialog;
public sealed class DialogCodec
{
    public required string Name { get; init; }

    public required int Id { get; init; }

    public required DialogElement Element { get; set; }
}
