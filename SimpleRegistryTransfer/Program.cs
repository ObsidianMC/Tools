using SimpleRegistryTransfer;
using System;
using System.Diagnostics;
using System.Reflection;

var jobs = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(x => typeof(IProcessJob).IsAssignableFrom(x) && x.IsClass && x.GetConstructor(Type.EmptyTypes) != null)
    .Select(x => (IProcessJob)Activator.CreateInstance(x))
    .ToList();

jobs.AddRange([
    new BiomeVariantJob("cat_variant"),
    new BiomeVariantJob("chicken_variant"),
    new BiomeVariantJob("cow_variant"),
    new BiomeVariantJob("frog_variant"),
    new BiomeVariantJob("pig_variant"),
]);

Directory.CreateDirectory(Helpers.OutputPath);

await SetupEnviromentAsync();

await using var sr = Helpers.RegistriesFilePath.OpenRead();
Helpers.Registries = await JsonSerializer.DeserializeAsync<JsonElement>(sr);

WriteLine("Starting jobs...");
foreach(var job in jobs)
{
    var jobName = job.GetType().Name;
    WriteLine("Starting {0}...", jobName);

    var timer = Stopwatch.StartNew();

    await job.Run();

    timer.Stop();

    WriteLine("Finished processing in {0}ms...", timer.ElapsedMilliseconds);
}