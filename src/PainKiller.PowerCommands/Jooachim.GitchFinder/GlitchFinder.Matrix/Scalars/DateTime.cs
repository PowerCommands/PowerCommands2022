using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.Scalars
{
    public class DateTime : IScalar
    {
        private System.DateTime _value;

        public System.DateTime Value => _value;

        public bool IsValid { get; set; }

        public DateTime(System.DateTime dateTime)
        {
            _value = dateTime;
            IsValid = true;
        }

        public static bool TryParse(string value, out IScalar parsed)
        {
            if(!System.DateTime.TryParse(value, out var parsedValue))
            {
                parsed = null;
                return false;
            }
            parsed = new DateTime(parsedValue);
            return true;
        }

        public bool IsEqual(IScalar otherValue)
        {
            if (otherValue is not DateTime otherDateTime)
                return false;
            if(_value == otherDateTime.Value)
                return true;

            IsValid = false;
            otherValue.IsValid = false;
            return false;
        }

        public override string ToString()
        {
            return _value.ToString("yyyy-MM-dd HH:mm:ss");
        }
        
    }
}
