using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Seeder.Converters;

public class TimeOnlyConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(TimeOnly);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        throw new NotSupportedException("This converter is write-only.");
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        var time = (TimeOnly)value!;
        emitter.Emit(new YamlDotNet.Core.Events.Scalar(time.ToString("HH:mm:ss", CultureInfo.InvariantCulture)));
    }
}
