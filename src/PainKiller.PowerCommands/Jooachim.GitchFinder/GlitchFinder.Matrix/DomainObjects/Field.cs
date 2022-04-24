using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.Matrix.DomainObjects
{
    public class Field : IField
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
