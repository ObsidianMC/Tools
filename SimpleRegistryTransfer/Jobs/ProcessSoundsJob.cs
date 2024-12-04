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

        var soundsFile = new FileInfo(Path.Combine(Helpers.OutputPath, "sounds.json"));

        if (soundsFile.Exists)
            soundsFile.Delete();

        using var soundWriter = soundsFile.OpenWrite();

        await JsonSerializer.SerializeAsync(soundWriter, sounds);
    }
}
