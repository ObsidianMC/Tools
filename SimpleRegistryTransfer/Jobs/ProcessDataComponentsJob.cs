using System.Collections.Generic;

namespace SimpleRegistryTransfer.Jobs;

public sealed class ProcessDataComponentsJob : IProcessJob
{
    public async ValueTask Run()
    {
        var registry = Helpers.Registries.GetProperty("minecraft:data_component_type");

        var entries = registry.GetProperty("entries");

        var dataComponents = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(entries.ToString());

        var dataComponentsFile = new FileInfo(Path.Combine(Helpers.OutputPath, "data_components.json"));

        if (dataComponentsFile.Exists)
            dataComponentsFile.Delete();

        using var dataComponentsWriter = dataComponentsFile.OpenWrite();

        await JsonSerializer.SerializeAsync(dataComponentsWriter, dataComponents);
    }
}
