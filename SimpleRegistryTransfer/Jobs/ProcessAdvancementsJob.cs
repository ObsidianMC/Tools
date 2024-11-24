using System.Collections.Generic;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessAdvancementsJob : IProcessJob
{
    public async ValueTask Run()
    {
        var dict = new Dictionary<string, JsonElement>();
        foreach (var file in Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "advancement"), "*.json", SearchOption.AllDirectories))
        {
            var fi = new FileInfo(file);
            var name = fi.Name.Replace(".json", string.Empty);
            var directoryParent = fi.Directory.Parent;
            var currentDirectory = fi.Directory;

            string rootName;
            if (directoryParent is not null && (directoryParent.Name == "recipes" || directoryParent.Parent?.Name == "recipes"))
                rootName = $"minecraft:{directoryParent.Name}/{currentDirectory.Name}/{name}";
            else
                rootName = $"minecraft:{currentDirectory.Name}/{name}";

            var element = await JsonSerializer.DeserializeAsync<JsonElement>(fi.OpenRead());

            dict.Add(rootName, element);
        }

        var outputFile = new FileInfo(Path.Combine(Helpers.OutputPath, "advancements.json"));

        await JsonSerializer.SerializeAsync(outputFile.Open(FileMode.OpenOrCreate), dict, Helpers.CodecJsonOptions);
    }
}
