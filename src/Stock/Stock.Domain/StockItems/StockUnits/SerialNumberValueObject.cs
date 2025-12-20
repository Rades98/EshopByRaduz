using DomainObjects;
using System.Text.RegularExpressions;

namespace Stock.Domain.StockItems.StockUnits
{
    public sealed class SerialNumberValueObject(string value) : ValueObject<SerialNumberValueObject, string>(value), IValueObject<SerialNumberValueObject, string>
    {
        public const int MaxLength = 50;
        public const string Mask = @"^SN-\d{3}-\d{2}$";

        public static Result<SerialNumberValueObject> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result<SerialNumberValueObject>.Failure("SERIAL_ERROR_NULL_OR_EMPTY");
            }

            if (!Regex.IsMatch(value, Mask))
            {
                return Result<SerialNumberValueObject>.Failure("SERIAL_ERROR_INVALID_FORMAT");
            }

            if (value.Length > MaxLength)
            {
                return Result<SerialNumberValueObject>.Failure("SERIAL_ERROR_INVALID_LENGTH");
            }

            return Result<SerialNumberValueObject>.Success(new(value));
        }
    }
}
