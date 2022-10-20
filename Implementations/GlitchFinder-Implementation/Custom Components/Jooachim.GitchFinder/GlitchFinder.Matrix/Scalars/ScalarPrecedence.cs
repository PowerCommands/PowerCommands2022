using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GlitchFinder.Scalars
{
    public class ScalarPrecedence
    {
        private static List<Type> _scalarPrecedence = new List<Type>() { typeof(Scalars.DateTime), typeof(Scalars.Integer), typeof(Scalars.Decimal), typeof(Scalars.String) };

        public static List<Type> Precedence { get => _scalarPrecedence; set => _scalarPrecedence = value; }
    }
}
