using System.Collections.Generic;
using System.Text;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessBlockEntitiesJob : IProcessJob
{
    public async ValueTask Run()
    {
        var registry = Helpers.Registries.GetProperty("minecraft:block_entity_type");
        var entries = registry.GetProperty("entries");

        var nlockEntities = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(entries.ToString());

        var sb = new StringBuilder();
        foreach (var (name, _) in nlockEntities)
            sb.AppendLine($"\"{name}\",");

        var blockEntityFile = new FileInfo(Path.Combine(Helpers.OutputPath, "block_entities.txt"));

        using var blockEntityWriter = new StreamWriter(blockEntityFile.OpenWrite());

        await blockEntityWriter.WriteLineAsync(sb.ToString());
    }
}
