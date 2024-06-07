namespace SimpleRegistryTransfer.Entities.Codecs.Biome;

public class BiomeParticle
{
    public float Probability { get; set; }

    public BiomeOption Options { get; set; }
}

public class BiomeOption
{
    public string Type { get; set; }
}