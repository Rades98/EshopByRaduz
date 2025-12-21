namespace Seeder.Models;

public class SeedMemberMetadata
{
    public required string TableName { get; set; }

    public bool IsDevTestOnly { get; set; }

    public bool Force { get; set; }

    public bool AllowIdentityInsert { get; set; }

    public string AdditionalUseCaseName { get; set; } = null!;
}
