﻿namespace SimpleRegistryTransfer.Entities.Codecs.Biome;

public class BiomeMusicEffect
{
    public bool ReplaceCurrentMusic { get; set; }

    public int MaxDelay { get; set; }

    public string Sound { get; set; }

    public int MinDelay { get; set; }
}