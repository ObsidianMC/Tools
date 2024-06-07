using System.Collections.Generic;

namespace SimpleRegistryTransfer.Entities.Codecs.ChatType;
public sealed record class ChatTypeCodec
{
    public required string Name { get; init; }

    public required int Id { get; init; }

    public required ChatElement Element { get; init; }
}
