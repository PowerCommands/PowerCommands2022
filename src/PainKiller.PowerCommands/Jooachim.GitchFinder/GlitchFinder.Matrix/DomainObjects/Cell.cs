using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.Matrix.DomainObjects
{
    public class Cell : ICell
    {
        public Cell(IScalar value)
        {
            Value = value;
            CellAttribute = CellAttribute.None;
        }

        public IScalar Value { get; set; }
        public CellAttribute CellAttribute { get; set; }
    }
}
