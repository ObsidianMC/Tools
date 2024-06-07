using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.ArmorTrim.TrimMaterial;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessTrimMaterialsJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<TrimMaterialCodec> trimMaterialCodec = new()
        {
            Type = "minecraft:trim_material",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "trim_material"), "*.json"))
        {
            var trimMaterialFile = new FileInfo(file);

            var trimMaterialName = trimMaterialFile.Name.Replace(".json", string.Empty);

            await using var trimMaterialStream = trimMaterialFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<TrimMaterialElement>(trimMaterialStream, Helpers.CodecJsonOptions);

            trimMaterialCodec.Value.Add(new TrimMaterialCodec
            {
                Name = $"minecraft:{trimMaterialName}",
                Element = element,
                Id = id++
            });
        }

        trimMaterialCodec.Value = [.. trimMaterialCodec.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "trim_material.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(trimMaterialCodec, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}
