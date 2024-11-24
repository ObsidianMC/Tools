using SimpleRegistryTransfer;

public partial class Program
{
    public static async Task SetupEnviromentAsync()
    {
        WriteLine("Retrieving latest version...");

        var versionManifest = await Helpers.HttpClient.GetFromJsonAsync<JsonElement>(Helpers.VersionManifestUrl, Helpers.JsonOptions);

        var latestProperty = versionManifest.GetProperty("latest");
        var latestVersion = latestProperty.GetProperty("release").GetString();

        WriteLine("Latest version found... ({0})", latestVersion);

        Helpers.LatestVersion = latestVersion;
        var latestPath = Path.Combine(Helpers.OutputPath, latestVersion);

        Helpers.EnviromentPath = latestPath;

        WriteLine("Enviroment Path: {0}", latestPath);

        if (Directory.Exists(latestPath))
        {
            WriteLine("Latest path exists. Starting processing...");
            return;
        }

        Directory.CreateDirectory(latestPath);

        var serverJarPath = Path.Combine(latestPath, "server.jar");

        var versionProperty = versionManifest.GetProperty("versions")
            .EnumerateArray()
            .FirstOrDefault(x => x.TryGetProperty("id", out var v) && v.GetString() == latestVersion);

        var versionJson = await Helpers.HttpClient.GetFromJsonAsync<JsonElement>(versionProperty.GetProperty("url").GetString(), Helpers.JsonOptions);

        var serverProperty = versionJson.GetProperty("downloads").GetProperty("server");

        WriteLine("Downloading server jar...");

        await using var serverJar = await Helpers.HttpClient.GetStreamAsync(serverProperty.GetProperty("url").GetString());

        if (File.Exists(serverJarPath))
            File.Delete(serverJarPath);

        WriteLine("Copying server jar...");

        await using var serverJarFile = new FileStream(serverJarPath, FileMode.CreateNew);
        await serverJar.CopyToAsync(serverJarFile);

        WriteLine("Running data generators...");

        await Cli.Wrap("java")
            .WithArguments("-DbundlerMainClass=net.minecraft.data.Main -jar server.jar --all")
            .WithStandardOutputPipe(PipeTarget.ToDelegate(WriteLine))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(WriteLine))
            .WithWorkingDirectory(latestPath)
            .ExecuteAsync();
    }
}
