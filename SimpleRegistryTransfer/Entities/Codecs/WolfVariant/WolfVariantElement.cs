using System.Collections.Generic;

namespace SimpleRegistryTransfer.Entities.Codecs.WolfVariant;
public sealed class WolfVariantElement
{
    public Dictionary<string, string> Assets { get; set; }

    public List<SpawnConditionElement> SpawnConditions { get; set; }
}