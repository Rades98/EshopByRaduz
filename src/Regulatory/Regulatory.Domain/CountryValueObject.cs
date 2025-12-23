using DomainObjects;

namespace Regulatory.Domain
{
    public class CountryValueObject :
        ValueObject<CountryValueObject, string>,
        IValueObject<CountryValueObject, string>
    {
        private CountryValueObject(string value) : base(value) { }

        public static Result<CountryValueObject> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result<CountryValueObject>.Failure("ERROR_COUNTRY_CODE_EMPTY");
            }
            if (value.Length != 2)
            {
                return Result<CountryValueObject>.Failure("ERROR_COUNTRY_CODE_2_CHARS");
            }

            return Result<CountryValueObject>.Success(new CountryValueObject(value.ToUpperInvariant()));
        }
    }
}
