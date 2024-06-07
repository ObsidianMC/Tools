using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.Biome;
using System.Collections.Generic;
using System.Text;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessBiomesJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<BiomeCodec> biomeCodecs = new()
        {
            Type = "minecraft:worldgen/biome",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Helpers.BiomePath, "*.json"))
        {
            var biomeFile = new FileInfo(file);

            var biomeName = biomeFile.Name.Replace(".json", string.Empty);

            await using var biomeFileStream = biomeFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<BiomeElement>(biomeFileStream, Helpers.CodecJsonOptions);

            biomeCodecs.Value.Add(new BiomeCodec
            {
                Name = $"minecraft:{biomeName}",
                Element = element,
                Id = id++
            });
        }

        biomeCodecs.Value = [.. biomeCodecs.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "biome_codec.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(biomeCodecs, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();

        await this.ProcessBiomeIdsAsync(biomeCodecs.Value);
    }

    private async Task ProcessBiomeIdsAsync(List<BiomeCodec> codecs)
    {
        var sb = new StringBuilder();
        foreach (var codec in codecs)
        {
            var newName = Helpers.TextInfo.ToTitleCase(codec.Name.TrimResourceTag(true)).Replace("_", string.Empty);

            sb.AppendLine($"{newName} = {codec.Id},");
        }

        var biomesFile = new FileInfo(Path.Combine(Helpers.OutputPath, "biomes.txt"));

        using var biomesWriter = new StreamWriter(biomesFile.OpenWrite());

        await biomesWriter.WriteLineAsync(sb.ToString());
    }
}
