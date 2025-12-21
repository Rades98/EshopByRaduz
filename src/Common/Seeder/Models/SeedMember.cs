namespace Seeder.Models;

public class SeedMember
{
    public SeedMemberMetadata Metadata { get; set; } = null!;

    public List<object> Data { get; set; } = null!;
}
