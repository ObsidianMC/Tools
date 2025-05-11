using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs;

namespace SimpleRegistryTransfer;
public abstract class BiomeVariantJob(string identifier) : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<BiomeVariantCodec> biomeVariantCodec = new()
        {
            Type = $"minecraft:{identifier}",
            Value = []
        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, identifier), "*.json"))
        {
            var variantFile = new FileInfo(file);

            var variantName = variantFile.Name.Replace(".json", string.Empty);

            await using var variantFileStream = variantFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<BiomeVariantElement>(variantFileStream, Helpers.CodecJsonOptions);

            biomeVariantCodec.Value.Add(new BiomeVariantCodec
            {
                Name = $"minecraft:{variantName}",
                Element = element,
                Id = id++
            });
        }

        biomeVariantCodec.Value = [.. biomeVariantCodec.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, $"{identifier}.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(biomeVariantCodec, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}