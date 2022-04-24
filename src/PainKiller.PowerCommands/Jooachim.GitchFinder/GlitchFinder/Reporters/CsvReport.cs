using System;
using System.IO;
using System.Linq;
using GlitchFinder.Contracts;
using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.Reporters
{
    public class CsvReport : IGlitchReport
    {
        private string _separator;
        private string _notAvailable;

        public void CreateReport(string filePath, IMatrix comparison, bool isEqual)
        {
            _separator = ";";
            _notAvailable = "#N/A";

            var csv = DifferencesTable(comparison);

            using var streamWriter = new StreamWriter(filePath);
            streamWriter.Write(csv);
            streamWriter.Close();
        }

        public void NonUniqueKeys(string filePath, IMatrix leftMatrix, IMatrix rightMatrix, bool isEqual)
        {
            _separator = ";";
            _notAvailable = "#N/A";

            var csv = "";
            if (leftMatrix != null)
                csv += DifferencesTable(leftMatrix, "LeftMatrix");
            if (rightMatrix != null)
                csv += DifferencesTable(rightMatrix, "RightMatrix");

            using var streamWriter = new StreamWriter(filePath);
            streamWriter.Write(csv);
            streamWriter.Close();
        }

        private string DifferencesTable(IMatrix matrix, string tableName = null)
        {
            var keys = matrix.Keys.OrderBy(k => k).ToList();
            if (keys.Count == 0)
                return "";

            var csv = "";
            var fields = matrix.Fields.Where(f => f.Name != "Key").ToList();
            if (!String.IsNullOrEmpty(tableName))
                csv += "TableName";
            foreach (var field in fields)
            {
                if (csv != "")
                    csv += _separator;
                csv += field.Name;
            }
            csv += "\n";

            foreach (var key in keys)
            {
                if (!matrix.TryGetRow(key, out var row))
                    throw new Exception("Inconsistent matrix");

                var outRow = "";
                if (!String.IsNullOrEmpty(tableName))
                    outRow += tableName;
                foreach (var field in fields)
                {
                    if (outRow != "")
                        outRow += _separator;
                    if (row.TryGetValue(field.Name, out var value))
                        outRow += value.ToString();
                    else
                        outRow += _notAvailable;
                }
                csv += outRow + "\n";
            }

            return csv;
        }
    }
}
