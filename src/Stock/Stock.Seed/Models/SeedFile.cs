namespace Stock.Seed.Models;

public class SeedFile
{
    public required string Path { get; set; }

    public required int Order { get; set; }

    public bool IsProcedure { get; set; }
}
