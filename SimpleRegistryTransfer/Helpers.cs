using System.Globalization;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace SimpleRegistryTransfer;
public static class Helpers
{
    public const string OutputPath = "output";

    public const string VersionManifestUrl = "https://launchermeta.mojang.com/mc/game/version_manifest.json";

    public static string EnviromentPath { get; set; }

    public static string LatestVersion { get; set; }

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

    public static string RemoveNamespace(this string namespacedName)
    {
        return namespacedName.Substring(namespacedName.IndexOf(":") + 1);
    }

    public static string ToPascalCase(this string snakeCase)
    {
        // Alternative implementation:
        // var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        // return string.Join("", snakeCase.Split('_').Select(s => textInfo.ToTitleCase(s)));

        int spaceCount = 0;
        for (int i = 0; i < snakeCase.Length; i++)
        {
            if (!char.IsLetterOrDigit(snakeCase[i]))
                spaceCount++;
        }

        var result = new char[snakeCase.Length - spaceCount];

        int targetIndex = 0;
        bool wordStart = true;
        for (int i = 0; i < snakeCase.Length; i++)
        {
            char c = snakeCase[i];
            if (char.IsLetterOrDigit(c))
            {
                result[targetIndex++] = wordStart ? char.ToUpper(c) : char.ToLower(c);
                wordStart = false;
            }
            else
            {
                wordStart = true;
            }
        }

        return new string(result);
    }
}
