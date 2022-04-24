using System.Collections.Generic;
using System.Linq;
using GlitchFinder.Matrix.Contracts;
using GlitchFinder.Matrix.DomainObjects;

namespace GlitchFinder.Matrix
{
    public class MatrixComparer
    {
        static string leftPrefix = "Left/";
        static string rightPrefix = "Right/";

        public static bool IsEqual(List<ComparisonField> compareFields, IMatrix leftMatrix, IMatrix rightMatrix, out IMatrix differences)
        {
            if(compareFields == null || compareFields.Count == 0)
            {
                // Assuming we're trying to compare two basically identical tables
                for(int i = 0; i < leftMatrix.Fields.Count && i < rightMatrix.Fields.Count; i++)
                {
                    compareFields.Add(new ComparisonField { LeftFieldName = leftMatrix.Fields[i].Name, RightFieldName = rightMatrix.Fields[i].Name });
                }
            }

            differences = new DomainObjects.Matrix();

            differences.UniqueKeys = new List<string> { "Key" };
            differences.Fields = new List<Field> { new Field { Name = "Key", Type = typeof(Scalars.String).ToString() } };


            var leftFields = new List<Field>();
            foreach (var keyConstituent in leftMatrix.UniqueKeys)
            {
                leftFields.Add(new Field { Name = leftPrefix + keyConstituent, Type = leftMatrix.Fields.Where(f => f.Name == keyConstituent).Single().Type });
            }
            var rightFields = new List<Field>();
            foreach (var keyConstituent in rightMatrix.UniqueKeys)
            {
                rightFields.Add(new Field { Name = rightPrefix + keyConstituent, Type = rightMatrix.Fields.Where(f => f.Name == keyConstituent).Single().Type });
            }
            foreach (var compareField in compareFields)
            {
                var leftField = leftMatrix.Fields.Where(f => f.Name == compareField.LeftFieldName).Single();
                leftFields.Add(new Field { Name = leftPrefix + leftField.Name, Type = leftField.Type });
                var rightField = rightMatrix.Fields.Where(f => f.Name == compareField.RightFieldName).Single();
                rightFields.Add(new Field { Name = rightPrefix + rightField.Name, Type = rightField.Type });
            }
            differences.Fields = differences.Fields.Union(leftFields).Union(rightFields).ToList();

            foreach (var key in leftMatrix.Keys.Union(rightMatrix.Keys))
            {
                bool identical = true;
                identical &= leftMatrix.TryGetRow(key, out var leftRow);
                identical &= rightMatrix.TryGetRow(key, out var rightRow);
                if (identical)
                {
                    foreach (var compareField in compareFields)
                    {
                        var leftFieldName = compareField.LeftFieldName;
                        var leftValue = leftRow[leftFieldName];

                        var rightFieldName = compareField.RightFieldName;
                        var rightValue = rightRow[rightFieldName];

                        var valuesAreEqual = leftValue.Value.IsEqual(rightValue.Value);
                        if (!valuesAreEqual)
                        {
                            leftValue.CellAttribute |= CellAttribute.Marked;
                            rightValue.CellAttribute |= CellAttribute.Marked;
                        }

                        identical &= valuesAreEqual;
                    }
                }

                if (!identical)
                {
                    var compositeRow = JoinRow(key, leftRow, rightRow);
                    differences.SetRow(key, compositeRow);
                }
            }

            return !differences.Keys.Any(); ;
        }

        private static Dictionary<string, ICell> JoinRow(string key, Dictionary<string, ICell> leftRow, Dictionary<string, ICell> rightRow)
        {
            var row = new Dictionary<string, ICell>();
            row.Add("Key", new Cell(new Scalars.String(key)));

            if (leftRow != null)
            {
                foreach (var kv in leftRow)
                {
                    row.Add("Left/" + kv.Key, kv.Value);
                }
            }
            if (rightRow != null)
            {
                foreach (var kv in rightRow)
                {
                    row.Add("Right/" + kv.Key, kv.Value);
                }
            }

            return row;
        }

    }
}
