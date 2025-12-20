using Stock.Seed.Models;

namespace Stock.Seed.Models.FileEntity;

internal sealed class SeedMemberFile
{
    public SeedMemberMetadata Metadata { get; set; } = null!;

    public List<SeedFileData> Data { get; set; } = null!;
}
