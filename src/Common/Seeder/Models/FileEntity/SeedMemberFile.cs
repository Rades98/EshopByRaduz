namespace Seeder.Models.FileEntity;

public sealed class SeedMemberFile
{
    public SeedMemberMetadata Metadata { get; set; } = null!;

    public List<SeedFileData> Data { get; set; } = null!;
}
