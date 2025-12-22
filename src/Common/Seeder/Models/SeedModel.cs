using System.Collections.ObjectModel;

namespace Seeder.Models;

public sealed class SeedModel
{
    public Collection<SeedFile> Files { get; set; } = [];
}
