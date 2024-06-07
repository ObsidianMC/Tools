using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.WolfVariant;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessWolfVariantsJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<WolfVariantCodec> wolfVariantCodec = new()
        {
            Type = "minecraft:wolf_variant",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "wolf_variant"), "*.json"))
        {
            var wolfVariantFile = new FileInfo(file);

            var wolfVariantName = wolfVariantFile.Name.Replace(".json", string.Empty);

            await using var wolfVariantFileStream = wolfVariantFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<WolfVariantElement>(wolfVariantFileStream, Helpers.CodecJsonOptions);

            wolfVariantCodec.Value.Add(new WolfVariantCodec
            {
                Name = $"minecraft:{wolfVariantName}",
                Element = element,
                Id = id++
            });
        }

        wolfVariantCodec.Value = [.. wolfVariantCodec.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "wolf_variant.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(wolfVariantCodec, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}
