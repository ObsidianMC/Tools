using System.Collections.Generic;

namespace SimpleRegistryTransfer.Jobs;
public sealed class ProcessPacketsJob : IProcessJob
{
    

    public async ValueTask Run()
    {
        var packetsFile = new FileInfo(Path.Combine(Helpers.ReportsPath, "packets.json"));

        if (!packetsFile.Exists)
            return;

        var fi = new FileInfo(Path.Combine(Helpers.OutputPath, "packets.json"));

        if (fi.Exists)
            fi.Delete();

        await using var sr = packetsFile.OpenRead();
        var packets = await GetPacketsAsync(sr);

        await using var sw = fi.Open(FileMode.CreateNew);

        await JsonSerializer.SerializeAsync(sw, packets, Helpers.CodecJsonOptions);
    }

    private static async Task<List<Packet>> GetPacketsAsync(Stream packetsStream)
    {
        var root = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(packetsStream)!;
        var list = new List<Packet>();

        foreach (var kv in root)
        {
            var state = kv.Key.ToPascalCase();
            var directionElement = kv.Value;

            foreach (var property in directionElement.EnumerateObject())
            {
                var @namespace = property.Name.ToPascalCase();
                var packets = property.Value;

                foreach (var packet in packets.EnumerateObject())
                {
                    var resourceId = packet.Name.RemoveNamespace();

                    var name = resourceId.ToPascalCase();
                    var packetId = packet.Value.GetProperty("protocol_id").GetInt32();

                    list.Add(new(name, resourceId, @namespace, state, packetId));
                }
            }
        }

        return list;
    }

    private readonly struct Packet(string name, string resourceId, string @namespace, string state, int packetId)
    {
        public string Name { get; } = name;

        public string ResourceId { get; } = resourceId;

        public string Namespace { get; } = @namespace;

        public string State { get; } = state;

        public int PacketId { get; } = packetId;

        public string UsableInterface => $"I{Namespace}Packet";
    }
}
