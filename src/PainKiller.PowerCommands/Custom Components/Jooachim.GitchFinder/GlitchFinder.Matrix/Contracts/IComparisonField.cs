using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlitchFinder.Matrix.Contracts
{
    public interface IComparisonField
    {
        string LeftFieldName { get; set; }
        string RightFieldName { get; set; }
    }
}
