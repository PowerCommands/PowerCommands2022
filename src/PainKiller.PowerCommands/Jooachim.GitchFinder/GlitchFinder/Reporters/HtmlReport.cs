using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GlitchFinder.Contracts;
using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.Reporters
{
    public class HtmlReport : IGlitchReport
    {
        public void CreateReport(string filePath, IMatrix comparison, bool isEqual)
        {
            var html = BuildHtml(comparison, isEqual);

            using var streamWriter = new StreamWriter($"{filePath}");
            streamWriter.Write(html);
            streamWriter.Close();
        }

        public string BuildHtml(IMatrix comparison, bool isEqual)
        {
            var html = new StringBuilder();
            html.Append($"<html><head>\n{styleSheet}\n</head><body>");
            html.Append($"Tables are {(isEqual ? "identical." : "different!")}<br><br>");
            html.Append(DifferencesTable(comparison));
            html.Append("</body></html>\n");

            return html.ToString();
        }

        public void NonUniqueKeys(string filePath, IMatrix leftMatrix, IMatrix rightMatrix, bool isEqual)
        {

            var html = new StringBuilder();
            html.Append($"<html><head>\n{styleSheet}\n</head><body>");
            if(leftMatrix != null)
            {
                html.Append("Table have non-unique keys.<br><br>");
                html.Append(DifferencesTable(leftMatrix));
            }
            if (rightMatrix != null)
            {
                html.Append("<br>Table have non-unique keys.<br><br>");
                html.Append(DifferencesTable(rightMatrix));
            }
            html.Append("</body></html>\n");

            using var streamWriter = new StreamWriter($"{filePath}.html");
            streamWriter.Write(html.ToString());
            streamWriter.Close();
        }

        private string DifferencesTable(IMatrix matrix)
        {
            var keys = matrix.Keys.OrderBy(k => k).ToList();
            if (keys.Count == 0)
                return "";

            var table = new StringBuilder();
            table.Append("<table>\n");
            var fields = matrix.Fields.Where(f => f.Name != "Key").ToList();
            table.Append("<tr>");
            foreach (var field in fields)
            {
                table.Append($"<th>{field.Name}<br>({TrimFieldType(field.Type.ToString())})</th>");
            }
            table.Append("</tr>\n");

            foreach(var key in keys)
            {
                if (!matrix.TryGetRow(key, out var row))
                    throw new Exception("Inconsistent matrix");

                table.Append("<tr>");
                foreach (var field in fields)
                {
                    if (row.TryGetValue(field.Name, out var value))
                    {
                        var isMarked = (value.CellAttribute & CellAttribute.Marked) == CellAttribute.Marked;
                        table.Append($"<td{(isMarked ? " class=marked" : "")}>{value.Value}</td>");
                    }
                    else
                        table.Append("<td>-</td>");
                }
                table.Append("</tr>\n");
            }
            table.Append("</table>\n");

            return table.ToString();
        }

        private string TrimFieldType(string fieldType)
        {
            var match = Regex.Match(fieldType, @"^([^.]+\.)*?([^.]+)$");
            if (!match.Success)
                return fieldType;
            return match.Groups[2].Captures[0].ToString();
        }


        private const string styleSheet =
            @"
<style>
    td.marked {background-color:#fcf;}
</style>
";
    }
}
