using System;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessRecipesJob : IProcessJob
{
    public async ValueTask Run()
    {
        var files = Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "recipe"), "*.json", SearchOption.AllDirectories);
        var recipesFile = new FileInfo(Path.Combine(Helpers.OutputPath, "recipes.json"));

        if (recipesFile.Exists)
            recipesFile.Delete();

        await using var sw = recipesFile.Create();
        await using var writer = new Utf8JsonWriter(sw, new JsonWriterOptions() { Indented = true });

        writer.WriteStartObject();

        WriteLine($"Processing {files.Length} recipes.");
        foreach (var file in files.Select(x => new FileInfo(x)))
        {
            var recipeName = Path.GetFileNameWithoutExtension(file.Name);

            await using var sr = file.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<JsonElement>(sr);

            writer.WritePropertyName(recipeName);
            writer.WriteRawValue(element.ToString());
        }

        writer.WriteEndObject();
    }
}
