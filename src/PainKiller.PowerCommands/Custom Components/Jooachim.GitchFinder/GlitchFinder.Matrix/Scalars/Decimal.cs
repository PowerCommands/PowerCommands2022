
using GlitchFinder.Matrix.Contracts;


namespace GlitchFinder.Scalars
{
    public class Decimal : IScalar
    {
        private decimal _value;

        public decimal Value => _value;

        public bool IsValid { get; set; }

        public Decimal(decimal value)
        {
            _value = value;
            IsValid = true;
        }

        public bool IsEqual(IScalar otherValue)
        {
            if (otherValue is not Decimal otherDecimal)
                return false;
            if(_value == otherDecimal.Value)
                return true;

            IsValid = false;
            otherValue.IsValid = false;
            return false;
        }

        public static bool TryParse(string value, out IScalar parsed)
        {
            if (!decimal.TryParse(value, out var parsedValue))
            {
                parsed = null;
                return false;
            }
            parsed = new Decimal(parsedValue);
            return true;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
