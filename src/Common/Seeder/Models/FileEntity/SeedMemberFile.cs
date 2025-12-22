using System.Collections.ObjectModel;

namespace Seeder.Models.FileEntity;

public sealed class SeedMemberFile
{
    public SeedMemberMetadata Metadata { get; set; } = null!;

    public Collection<SeedFileData> Data { get; } = null!;
}
