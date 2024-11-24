namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessItemsJob : IProcessJob
{
    public async ValueTask Run()
    {
        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "items.json"));

        using var sw = new StreamWriter(fi.OpenWrite());

        await sw.WriteLineAsync(Helpers.Registries.GetProperty("minecraft:item").ToString());
        await sw.FlushAsync();
    }
}
