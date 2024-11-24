using System.Collections.Generic;
using System.Text;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessParticlesJob : IProcessJob
{
    public async ValueTask Run()
    {
        var registry = Helpers.Registries.GetProperty("minecraft:particle_type");
        var entries = registry.GetProperty("entries");

        var particles = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(entries.ToString());

        var sb = new StringBuilder();
        foreach (var (name, particle) in particles.OrderBy(x => x.Value.GetProperty("protocol_id").GetInt32()))
        {
            var newName = Helpers.TextInfo.ToTitleCase(name);

            sb.AppendLine($"{newName.TrimResourceTag()} = {particle.GetProperty("protocol_id").GetInt32()},");
        }

        var particlesFile = new FileInfo(Path.Combine(Helpers.OutputPath, "particles.txt"));

        using var particlesWriter = new StreamWriter(particlesFile.OpenWrite());

        await particlesWriter.WriteLineAsync(sb.ToString());
    }
}
