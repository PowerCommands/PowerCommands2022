#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.DataSources
{
    public class CsvFileImport
    {
        public bool TryImport(SourceSettingContainer sourceSettings, out IMatrix matrix)
        {
            var fss = sourceSettings.ToCsvFileSourceSetting();
            ReadRawMatrix(fss, out List<string> fields, out List<List<string>> rawMatrix);
            return MatrixCreator.ParseMatrix(fields, fss.UniqueKeyFields.ToList(), rawMatrix, out matrix);
        }

        private void ReadRawMatrix(CsvFileSourceSetting fss, out List<string> fields, out List<List<string>> rawMatrix)
        {
            rawMatrix = ReadFile(fss);

            fields = new List<string>();

            if (!fss.Header)
            {
                var noColumns = GetMaxColumns(rawMatrix);
                for(var c = 0; c < noColumns; c++)
                    fields.Add("Col" + c);
            }
            else
            {
                if (rawMatrix.Count == 0 && fss.ReplaceHeader == null)
                    throw new Exception("Empty file, can't read header row.");
                var headerRow = rawMatrix.First();
                rawMatrix.RemoveAt(0);
                if (fss.ReplaceHeader != null)
                    headerRow = fss.ReplaceHeader;

                fields.AddRange(headerRow.Select(field => field ));
            }
        }

        private List<List<string>> ReadFile(CsvFileSourceSetting fss)
        {
            var rawMatrix = new List<List<string>>();
            try
            {
                foreach (var line in System.IO.File.ReadAllLines(fss.FilePath))
                {
                    rawMatrix.Add(line.Split(fss.Separator).Select(s => { if (fss.Trim) s = s.Trim(); return s; }).ToList());
                }
            }
            catch
            {
                throw new Exception($"Failed reading and splitting file {fss.FilePath} with separator {fss.Separator}");
            }

            return rawMatrix;
        }

        public static int GetMaxColumns<T>(List<List<T>> matrix)
        {
            int maxColumns = 0;
            foreach (var row in matrix) // ToDo: where is resharper when you need it?
            {
                if (row.Count > maxColumns)
                    maxColumns = row.Count;
            }
            return maxColumns;
        }

    }
}
