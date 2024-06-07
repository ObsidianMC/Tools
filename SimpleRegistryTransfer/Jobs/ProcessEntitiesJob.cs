using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessEntitiesJob : IProcessJob
{
    public async ValueTask Run()
    {
        var registries = Helpers.Registries.GetProperty("minecraft:entity_type");
        var entries = registries.GetProperty("entries");

        var entities = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(entries.ToString());

        var sb = new StringBuilder();
        foreach (var (name, _) in entities.OrderBy(x => x.Value.GetProperty("protocol_id").GetInt32()))
        {
            var newName = Helpers.TextInfo.ToTitleCase(name);

            sb.AppendLine($"{newName.TrimResourceTag()},");
        }

        var entityTypesFile = new FileInfo(Path.Combine(Helpers.OutputPath, "entity_type.txt"));

        using var entityTypeWriter = new StreamWriter(entityTypesFile.OpenWrite());

        await entityTypeWriter.WriteLineAsync(sb.ToString());
    }
}
