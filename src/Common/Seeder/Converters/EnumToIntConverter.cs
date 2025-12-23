using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Seeder.Converters;

public class EnumToIntConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type is not null && type.IsEnum;

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        throw new NotSupportedException("This converter is write-only.");
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        _ = emitter is null ? throw new ArgumentNullException(nameof(emitter)) : "";

        var enumValue = value as Enum;
        emitter.Emit(new YamlDotNet.Core.Events.Scalar(Convert.ToInt32(enumValue, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)));
    }
}
