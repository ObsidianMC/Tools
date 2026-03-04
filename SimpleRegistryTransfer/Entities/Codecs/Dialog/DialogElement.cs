namespace SimpleRegistryTransfer.Entities.Codecs.Dialog;
public sealed class DialogElement
{
    public required string Type { get; set; }

    public required int ButtonWidth { get; set; }

    public required int Columns { get; set; }

    public string? Dialogs { get; set; }

    public required DialogAction ExitAction { get; init; }

    public required ChatMessage ExternalTitle { get; init; }

    public required ChatMessage Title { get; init; }
}

public sealed class DialogAction
{
    public required ChatMessage Label { get; init; }
    public required int Width { get; set; }
}

public sealed class ChatMessage
{
    public required string Translate { get; set; }
}