namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessTagsJob : IProcessJob
{
    public async ValueTask Run()
    {
        var files = Directory.GetFiles(Path.Combine(Helpers.MinecraftDataPath, "tags"), "*.json", SearchOption.AllDirectories);
        var tagsFile = new FileInfo(Path.Combine(Helpers.OutputPath, "tags.json"));

        if (tagsFile.Exists)
            tagsFile.Delete();

        await using var sw = tagsFile.Create();
        await using var writer = new Utf8JsonWriter(sw, new JsonWriterOptions() { Indented = true });

        writer.WriteStartObject();

        WriteLine($"Processing {files.Length} tags.");
        foreach (var file in files.Select(x => new FileInfo(x)))
        {
            var relativePath = Path.GetRelativePath(".", file.DirectoryName).Replace("output\\1.21\\generated\\data\\minecraft\\tags\\", string.Empty).Replace("\\", "/");
            var fileName = Path.GetFileNameWithoutExtension(file.Name);

            var tagName = $"{relativePath}/{fileName}";
            var tagType = relativePath;

            await using var sr = file.OpenRead();
            var element = await JsonSerializer.DeserializeAsync<JsonElement>(sr);

            writer.WriteStartObject(tagName);

            writer.WriteString("name", fileName);
            writer.WriteString("type", tagType);

            writer.WritePropertyName("values");
            writer.WriteRawValue(element.EnumerateObject().First().Value.ToString(), true);

            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }
}
