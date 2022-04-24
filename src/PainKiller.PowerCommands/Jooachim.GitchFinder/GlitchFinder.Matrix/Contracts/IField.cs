using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlitchFinder.Matrix.Contracts
{
    public interface IField
    {
        string Name { get; set; }
        string Type { get; set; }
    }
}
