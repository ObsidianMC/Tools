namespace SimpleRegistryTransfer.Entities.Codecs.ChatType;
public sealed record class ChatStyle
{
    public required string Color { get; init; }

    public bool Italic { get; init; }
}
