using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlitchFinder.Matrix.Contracts
{
    public interface ICell
    {
        IScalar Value { get; set; }
        CellAttribute CellAttribute { get; set; }
    }
}
