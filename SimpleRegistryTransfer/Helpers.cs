using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleRegistryTransfer;
public static class Helpers
{
    public const string OutputPath = "output";

    public const string VersionManifestUrl = "https://launchermeta.mojang.com/mc/game/version_manifest.json";

    public static string EnviromentPath { get; set; }

    public static string GeneratedPath => Path.Combine(EnviromentPath, "generated");
    public static string MinecraftDataPath => Path.Combine(GeneratedPath, "data", "minecraft");
    public static string ReportsPath => Path.Combine(GeneratedPath, "reports");
    public static string WorldgenDataPath => Path.Combine(MinecraftDataPath, "worldgen");
    public static string BiomePath => Path.Combine(WorldgenDataPath, "biome");
    public static string ConfiguredFeaturesPath => Path.Combine(WorldgenDataPath, "configured_feature");
    public static JsonElement Registries { get; set; }

    public static FileInfo RegistriesFilePath => new(Path.Combine(ReportsPath, "registries.json"));
    public static FileInfo BlocksFilePath => new(Path.Combine(ReportsPath, "blocks.json"));


    public static HttpClient HttpClient => new();

    public static JsonSerializerOptions CodecJsonOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower, false)
            }
    };

    public static JsonSerializerOptions JsonOptions  => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static TextInfo TextInfo => CultureInfo.CurrentCulture.TextInfo;
}
