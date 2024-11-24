using System.Collections.Generic;
using System.Text;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessBlocksJob : IProcessJob
{
    public async ValueTask Run()
    {
        var blocksRegistry = Helpers.Registries.GetProperty("minecraft:block");

        var blocksEntries = blocksRegistry.GetProperty("entries");

        var blocks = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(blocksEntries.ToString());

        var set = new HashSet<string>();

        foreach (var (name, _) in blocks.OrderBy(x => x.Value.GetProperty("protocol_id").GetInt32()))
        {
            var newName = Helpers.TextInfo.ToTitleCase(name);

            set.Add($"{newName.TrimResourceTag()},");
        }

        var itemRegistry = Helpers.Registries.GetProperty("minecraft:item");
        var itemEntries = itemRegistry.GetProperty("entries");

        var items = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(itemEntries.ToString());

        foreach (var (name, _) in items.OrderBy(x => x.Value.GetProperty("protocol_id").GetInt32()))
        {
            var newName = Helpers.TextInfo.ToTitleCase(name);

            set.Add($"{newName.TrimResourceTag()},");
        }

        var sb = new StringBuilder();
        foreach (var str in set)
            sb.AppendLine(str);

        var materialsFile = new FileInfo(Path.Combine(Helpers.OutputPath, "materials.txt"));

        using var materialWriter = new StreamWriter(materialsFile.OpenWrite());

        await materialWriter.WriteLineAsync(sb.ToString());
    }
}
