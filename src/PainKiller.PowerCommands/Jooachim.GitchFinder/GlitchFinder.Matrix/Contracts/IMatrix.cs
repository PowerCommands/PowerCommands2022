using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlitchFinder.Matrix.DomainObjects;

namespace GlitchFinder.Matrix.Contracts
{
    public interface IMatrix
    {
        List<Field> Fields { get; set; }
        List<string> UniqueKeys { get; set; }


        List<string> Keys { get; }

        void SetRow(string key, Dictionary<string, ICell> row);
        bool TryGetRow(string key, out Dictionary<string, ICell> row);
    }
}
