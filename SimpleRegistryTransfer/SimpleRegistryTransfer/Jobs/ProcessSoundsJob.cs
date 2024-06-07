using System.Collections.Generic;
using System.Text;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessSoundsJob : IProcessJob
{
    public async ValueTask Run()
    {
        var registry = Helpers.Registries.GetProperty("minecraft:sound_event");

        var entries = registry.GetProperty("entries");

        var sounds = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(entries.ToString());

        var sb = new StringBuilder();
        foreach (var (name, element) in sounds)
        {
            var newName = Helpers.TextInfo.ToTitleCase(name.Replace('.', '_'));

            var actualId = element.GetProperty("protocol_id").GetInt32();

            sb.AppendLine($"{newName.TrimResourceTag()} = {actualId + 1},");
        }

        var soundsFile = new FileInfo(Path.Combine(Helpers.OutputPath, "sounds.txt"));

        using var soundWriter = new StreamWriter(soundsFile.OpenWrite());

        await soundWriter.WriteLineAsync(sb.ToString());
    }
}
