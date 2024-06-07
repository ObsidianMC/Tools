using System.Collections.Generic;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessConfiguredFeaturesJob : IProcessJob
{
    public async ValueTask Run()
    {
        var treesJsonFile = new FileInfo(Path.Combine(Helpers.OutputPath, "trees.json"));
        await using var treesJsonFileStream = treesJsonFile.Create();

        var elements = new Dictionary<string, JsonElement>();

        foreach (var file in Directory.GetFiles(Helpers.ConfiguredFeaturesPath, "*.json"))
        {
            var feature = new FileInfo(file);

            var name = feature.Name.Replace(".json", string.Empty);
            await using var featureStream = feature.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<JsonElement>(featureStream, Helpers.CodecJsonOptions);

            var type = element.GetProperty("type").GetString();

            if (type != "minecraft:tree")
                continue;

            elements.Add($"minecraft:{name}", element.GetProperty("config"));
        }

        await JsonSerializer.SerializeAsync(treesJsonFileStream, elements, Helpers.CodecJsonOptions);
    }
}
