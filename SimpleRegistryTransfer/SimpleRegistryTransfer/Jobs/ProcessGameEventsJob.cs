using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessGameEventsJob : IProcessJob
{
    public async ValueTask Run()
    {
        var registry = Helpers.Registries.GetProperty("minecraft:game_event");
        var entries = registry.GetProperty("entries");

        var gameEvents = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(entries.ToString());

        var sb = new StringBuilder();
        foreach (var (name, _) in gameEvents.OrderBy(x => x.Value.GetProperty("protocol_id").GetInt32()))
        {
            var newName = Helpers.TextInfo.ToTitleCase(name);

            sb.AppendLine($"{newName.TrimResourceTag()},");
        }

        var gameEventsFile = new FileInfo(Path.Combine(Helpers.OutputPath, "game_events.txt"));

        using var gameEventsWriter = new StreamWriter(gameEventsFile.OpenWrite());

        await gameEventsWriter.WriteLineAsync(sb.ToString());
    }
}
