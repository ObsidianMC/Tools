using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.Dialog;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessDialogsJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<DialogCodec> dialogCodec = new()
        {
            Type = "minecraft:dialog",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "dialog"), "*.json"))
        {
            var dialogFile = new FileInfo(file);

            var dialogName = dialogFile.Name.Replace(".json", string.Empty);

            await using var dialogFileStream = dialogFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<DialogElement>(dialogFileStream, Helpers.CodecJsonOptions);

            dialogCodec.Value.Add(new DialogCodec
            {
                Name = $"minecraft:{dialogName}",
                Element = element,
                Id = id++
            });
        }

        dialogCodec.Value = [.. dialogCodec.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "dialog_codec.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(dialogCodec, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}
