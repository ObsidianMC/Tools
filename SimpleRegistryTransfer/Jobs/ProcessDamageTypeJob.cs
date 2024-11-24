using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.DamageType;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessDamageTypeJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<DamageTypeCodec> damageTypeCodec = new()
        {
            Type = "minecraft:damage_type",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "damage_type"), "*.json"))
        {
            var damageTypeFile = new FileInfo(file);

            var damageTypeName = damageTypeFile.Name.Replace(".json", string.Empty);

            await using var damageTypeFileStream = damageTypeFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<DamageTypeElement>(damageTypeFileStream, Helpers.CodecJsonOptions);

            damageTypeCodec.Value.Add(new DamageTypeCodec
            {
                Name = $"minecraft:{damageTypeName}",
                Element = element,
                Id = id++
            });
        }

        damageTypeCodec.Value = [.. damageTypeCodec.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "damage_type.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(damageTypeCodec, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}
