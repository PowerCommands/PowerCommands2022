using System.Collections.Generic;

namespace GlitchFinder.Matrix.DomainObjects
{
    internal class SerializableMatrix
    {
        public List<Field> Fields { get; set; }
        public List<string> UniqueKeys { get; set; }
        public Dictionary<string, List<string>> Matrix { get; set; }

    }
}
