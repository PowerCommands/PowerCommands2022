using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlitchFinder.Matrix.Contracts
{
    public interface IScalar
    {
        bool IsEqual(IScalar value);
        bool IsValid { get; set; }
    }
}
