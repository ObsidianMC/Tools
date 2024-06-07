using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.ArmorTrim.TrimPattern;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessTrimPatternsJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<TrimPatternCodec> trimPatternCodec = new()
        {
            Type = "minecraft:trim_pattern",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "trim_pattern"), "*.json"))
        {
            var trimPatternFile = new FileInfo(file);

            var trimPatternName = trimPatternFile.Name.Replace(".json", string.Empty);

            await using var trimMaterialStream = trimPatternFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<TrimPatternElement>(trimMaterialStream, Helpers.CodecJsonOptions);

            trimPatternCodec.Value.Add(new TrimPatternCodec
            {
                Name = $"minecraft:{trimPatternName}",
                Element = element,
                Id = id++
            });
        }

        trimPatternCodec.Value = [.. trimPatternCodec.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "trim_pattern.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(trimPatternCodec, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}
