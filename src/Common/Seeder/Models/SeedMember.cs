using System.Collections.ObjectModel;

namespace Seeder.Models;

public class SeedMember
{
    public SeedMemberMetadata Metadata { get; set; } = null!;

    public Collection<object> Data { get; set; } = null!;
}
