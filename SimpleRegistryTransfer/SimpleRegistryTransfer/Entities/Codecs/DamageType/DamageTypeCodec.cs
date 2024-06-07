using System.Text.Json.Serialization;

namespace SimpleRegistryTransfer.Entities.Codecs.DamageType;
public class DamageTypeCodec
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required DamageTypeElement Element { get; init; }
}

public class DamageTypeElement
{
    public DeathMessageType? DeathMessageType { get; init; }

    public required float Exhaustion { get; init; }

    public required string MessageId { get; init; }

    public required DamageScaling Scaling { get; init; }

    public DamageEffects? Effects { get; init; }
}

public enum DeathMessageType
{
    Default,
    FallVariants,
    IntentionalGameDesign,
}

public enum DamageScaling
{
    Never,
    WhenCausedByLivingNonPlayer,
    Always
}

public enum DamageEffects
{
    Hurt,
    Thorns,
    Drowning,
    Burning,
    Poking,
    Freezing
}