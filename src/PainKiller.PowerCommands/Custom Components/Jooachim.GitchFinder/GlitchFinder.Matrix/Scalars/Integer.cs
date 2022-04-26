
using GlitchFinder.Matrix.Contracts;


namespace GlitchFinder.Scalars
{
    public class Integer : IScalar
    {
        private int _value;
        public int Value => _value;

        public bool IsValid { get; set; }

        public Integer(int value)
        {
            IsValid = true;
            _value = value;
        }

        public bool IsEqual(IScalar otherValue)
        {
            if (otherValue is not Integer otherInteger)
                return false;

            if(_value == otherInteger.Value)
                return true;

            IsValid = false;
            otherValue.IsValid = false;
            return false;
        }

        public static bool TryParse(string value, out IScalar parsed)
        {
            if (!int.TryParse(value, out var parsedValue))
            {
                parsed = null;
                return false;
            }
            parsed = new Integer(parsedValue);
            return true;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
