using System.Text.Json;

namespace Seeder.Models.FileEntity;

public sealed class SeedFileData
{
    public Guid Id { get; set; }

    public string Container { get; set; } = null!;

    public string Filename { get; set; } = null!;

    public string Mime { get; set; } = null!;

    public int Size { get; set; }

    public string Checksum { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Dictionary<string, object> Metadata { get; } = [];

    public string MetadataJson => JsonSerializer.Serialize(Metadata);
}
