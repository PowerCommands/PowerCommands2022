using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

using GlitchFinder.Matrix;
using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.DataSources
{
    public class SqlImport 
    {
        public bool TryImport(SourceSettingContainer sourceSettings, out IMatrix matrix)
        {
            var dss = sourceSettings.ToDatabaseSourceSetting();
            ReadRawMatrix(dss, out List<string> fields, out List<List<string>> rawMatrix);

            return MatrixCreator.ParseMatrix(fields, dss.UniqueKeyFields.ToList(), rawMatrix, out matrix);
        }

        private void ReadRawMatrix(SqlSourceSetting dss, out List<string> fields, out List<List<string>> rawMatrix)
        {
            if (string.IsNullOrEmpty(dss.ConnectionString)) 
                throw new Exception("SqlImport needs ConnectionString");

            fields = new List<string>();

            string query;
            if (!string.IsNullOrEmpty(dss.Query))
                query = dss.Query; // If user supplies both query and query file, we give query priority
            else if (!string.IsNullOrEmpty(dss.QueryFile))
                query = ReadQuery(dss.QueryFile);
            else
                throw new Exception("SqlImport needs either Query or QueryFile");

            rawMatrix = new List<List<string>>();
            using (SqlConnection connection = new SqlConnection(dss.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        var dbType = reader.GetFieldType(i);
                        fields.Add(fieldName);
                    }

                    while (reader.Read())
                    {
                        var row = new List<string>();
                        for(int i=0; i<reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            row.Add(reader[fieldName].ToString());
                        }
                        rawMatrix.Add(row);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        private string ReadQuery(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch
            {
                throw new Exception("Failed reading query from dss.QueryFile");
            }
        }
    }
}
