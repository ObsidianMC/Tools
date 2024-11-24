using SimpleRegistryTransfer.Entities.Codecs.Biome;
using SimpleRegistryTransfer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessCommandArgumentTypeJob : IProcessJob
{
    public  async ValueTask Run()
    {
        var registry = Helpers.Registries.GetProperty("minecraft:command_argument_type");
        var entries = registry.GetProperty("entries");

        var argumentTypes = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(entries.ToString());

        var dict = new Dictionary<int, string>();
        var sb = new StringBuilder();
        foreach (var (name, value) in argumentTypes)
            dict.Add(value.GetProperty("protocol_id").GetInt32(), name);


        foreach (var (id, name) in dict.OrderBy(x => x.Key))
        {
            sb.AppendLine($"{name} = {id},");    
        }

        var blockEntityFile = new FileInfo(Path.Combine(Helpers.OutputPath, "argument_types.txt"));

        using var blockEntityWriter = new StreamWriter(blockEntityFile.OpenWrite());

        await blockEntityWriter.WriteLineAsync(sb.ToString());
    }
}
