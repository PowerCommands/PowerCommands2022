using GlitchFinder.Matrix.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlitchFinder.Matrix.Contracts
{
    public class ComparisonField  : IComparisonField
    {
        public string LeftFieldName { get; set; }
        public string RightFieldName { get; set; }
    }
}
