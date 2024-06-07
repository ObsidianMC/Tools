using SimpleRegistryTransfer.Entities;
using SimpleRegistryTransfer.Entities.Codecs.ChatType;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessChatTypesJob : IProcessJob
{
    public async ValueTask Run()
    {
        BaseCodec<ChatTypeCodec> chatTypeCodec = new()
        {
            Type = "minecraft:chat_type",
            Value = []

        };

        var id = 0;
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "chat_type"), "*.json"))
        {
            var chatTypeFile = new FileInfo(file);

            var chatTypeName = chatTypeFile.Name.Replace(".json", string.Empty);

            await using var chatTypeFileStream = chatTypeFile.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<ChatElement>(chatTypeFileStream, Helpers.CodecJsonOptions);

            chatTypeCodec.Value.Add(new ChatTypeCodec
            {
                Name = $"minecraft:{chatTypeName}",
                Element = element,
                Id = id++
            });
        }

        chatTypeCodec.Value = [.. chatTypeCodec.Value.OrderBy(x => x.Id)];

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "chat_type.json"));

        using var sw = new StreamWriter(fi.OpenWrite());
        var json = JsonSerializer.Serialize(chatTypeCodec, Helpers.CodecJsonOptions);

        await sw.WriteLineAsync(json);
        await sw.FlushAsync();
    }
}
