﻿namespace SimpleRegistryTransfer.Entities.Codecs.ChatType;
public sealed record class ChatElement
{
    public required ChatType Chat { get; set; }

    public required ChatType Narration { get; set; }
}
