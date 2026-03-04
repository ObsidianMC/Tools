namespace SimpleRegistryTransfer.Entities.Codecs.Biome
{
    public class BiomeEffect
    {
        public BiomeMusicEffectData[] Music { get; set; }

        public string GrassColorModifier { get; set; }
        public string AmbientSound { get; set; }

        public BiomeAdditionSound AdditionsSound { get; set; }
        public BiomeMoodSound MoodSound { get; set; }

        public BiomeParticle Particle { get; set; }

        public string FoliageColor { get; set; }
        public string SkyColor { get; set; }
        public string WaterFogColor { get; set; }
        public string FogColor { get; set; }
        public string WaterColor { get; set; }
        public string GrassColor { get; set; }
    }
}
