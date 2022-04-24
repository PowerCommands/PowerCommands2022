
using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.Scalars
{
    public class String : IScalar
    {
        private string _value;

        public string Value => _value;

        public bool IsValid { get; set; }


        public String(string value)
        {
            IsValid = true;
            _value = value;
        }

        public bool IsEqual(IScalar otherValue)
        {
            if (otherValue is not String otherString)
                return false;
            if(_value == otherString.Value)
                return true;

            IsValid = false;
            otherValue.IsValid = false;
            return false;
        }

        public static bool TryParse(string value, out IScalar parsed)
        {
            parsed = new String(value);
            return true;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
