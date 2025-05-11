namespace SimpleRegistryTransfer.Entities.Codecs;
public sealed class SpawnConditionElement
{
    public SpawnCondition? Condition { get; set; }

    public required int Priority { get; set; }

    public sealed class SpawnCondition
    {
        public required string Type { get; set; }

        public string? Biomes { get; set; }

        public string? Structures { get; set; }

        public SpawnConditionRange? Range { get; set; }
    }
}

public readonly record struct SpawnConditionRange
{
    public double Min { get; init; }
    public double Max { get; init; }
}