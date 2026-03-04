using System.Collections.Generic;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessCommandParsersJob : IProcessJob
{
    public async ValueTask Run()
    {
        var parsers = Helpers.Registries.GetProperty("minecraft:command_argument_type").Deserialize<JsonElement>();

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "command_parsers.json"));

        var dict = new Dictionary<string, int>();

        foreach (var prop in parsers.GetProperty("entries").EnumerateObject())
        {
            dict.Add(prop.Name, prop.Value.GetProperty("protocol_id").GetInt32());
        }

        if(fi.Exists)
            fi.Delete();

        await using var sw = fi.Create();

        await JsonSerializer.SerializeAsync(sw, dict);
    }
}
