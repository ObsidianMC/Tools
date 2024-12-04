namespace SimpleRegistryTransfer.Entities.Codecs.Biome;

public class BiomeMusicEffect
{
    public bool ReplaceCurrentMusic { get; set; }

    public int MaxDelay { get; set; }

    public string Sound { get; set; }

    public int MinDelay { get; set; }
}

public class BiomeMusicEffectData
{
    public BiomeMusicEffect Data { get; set; }

    public int Weight { get; set; }
}