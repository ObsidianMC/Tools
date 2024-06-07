using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.Biome;
using SimpleRegistryTransfer.Entities.Codecs.Dimension;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessDimensionsJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<DimensionCodec> dimensionCodec = new()
        {
            Type = "minecraft:dimension_type",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "dimension_type"), "*.json"))
        {
            var dimensionFile = new FileInfo(file);

            var dimensionName = dimensionFile.Name.Replace(".json", string.Empty);

            await using var dimensionFileStream = dimensionFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<DimensionElement>(dimensionFileStream, Helpers.CodecJsonOptions);

            dimensionCodec.Value.Add(new DimensionCodec
            {
                Name = $"minecraft:{dimensionName}",
                Element = element,
                Id = id++
            });
        }

        dimensionCodec.Value = [.. dimensionCodec.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "dimension_type.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(dimensionCodec, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}
