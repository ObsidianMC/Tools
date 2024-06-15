using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.PaintingVariant;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessPaintingVariantsJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<PaintingVariantCodec> biomeCodecs = new()
        {
            Type = "minecraft:painting_variant",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "painting_variant"), "*.json"))
        {
            var variantFile = new FileInfo(file);

            var variantName = variantFile.Name.Replace(".json", string.Empty);

            await using var variantFileStream = variantFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<PaintingVariantElement>(variantFileStream, Helpers.CodecJsonOptions);

            biomeCodecs.Value.Add(new PaintingVariantCodec
            {
                Name = $"minecraft:{variantName}",
                Element = element,
                Id = id++
            });
        }

        biomeCodecs.Value = [.. biomeCodecs.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "painting_variants.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(biomeCodecs, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}
