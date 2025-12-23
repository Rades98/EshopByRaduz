using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Seeder.Converters;

public class DateOnlyConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(DateOnly);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        throw new NotSupportedException("This converter is write-only.");
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        _ = emitter is null ? throw new ArgumentNullException(nameof(emitter)) : "";

        var date = (DateOnly)value!;
        emitter.Emit(new YamlDotNet.Core.Events.Scalar(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
    }
}
