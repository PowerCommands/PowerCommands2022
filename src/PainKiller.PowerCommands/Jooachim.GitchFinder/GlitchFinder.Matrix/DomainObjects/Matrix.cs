using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.Matrix.DomainObjects
{
    public class Matrix : IMatrix
    {
        public List<Field> Fields { get; set; }
        public List<string> UniqueKeys { get; set; }

        private Dictionary<string, Dictionary<string, ICell>> _matrix = new Dictionary<string, Dictionary<string, ICell>>();

        public List<string> Keys
        {
            get
            {
                return _matrix.Keys.ToList();
            }
        }

        public Matrix()
        {

        }

        public void SetRow(string key, Dictionary<string, ICell> row)
        {
            _matrix.Add(key, row);
        }

        public bool TryGetRow(string key, out Dictionary<string, ICell> row)
        {
            return _matrix.TryGetValue(key, out row);
        }

        // ToDo: Fix below! 
        #region serialize
        public string Serialize()
        {
            var sm = new SerializableMatrix();
            sm.Fields = Fields;
            sm.UniqueKeys = UniqueKeys;
            sm.Matrix = new Dictionary<string, List<string>>();
            foreach(var key in Keys)
            {
                var serializedRow = new List<string>();

                TryGetRow(key, out var row);                
                foreach(var field in Fields)
                {
                    var value = row[field.Name];
                    serializedRow.Add(value.Value.ToString());
                }
                sm.Matrix.Add(key, serializedRow);
            }

            var jsonSerializerOptions = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() }, WriteIndented = true };
            var jsData = JsonSerializer.Serialize(sm, jsonSerializerOptions);
            return jsData;
        }

        public Matrix(string serializedMatrix)
        {
            var jsonSerializerOptions = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() }, WriteIndented = true };
            SerializableMatrix sm = JsonSerializer.Deserialize<SerializableMatrix>(serializedMatrix, jsonSerializerOptions);

            Fields = sm.Fields;
            UniqueKeys = sm.UniqueKeys;
            foreach(var serializedRow in sm.Matrix)
            {
                var row = new Dictionary<string, ICell>();
                for(int i = 0; i < Fields.Count; i++)
                {
                    var fieldName = Fields[i].Name;
                    var fieldType = Fields[i].Type;
                    var value = serializedRow.Value[i];
                    var cell = new Cell(CreateScalar(fieldType, value));
                    row.Add(fieldName, cell);
                }

                SetRow(serializedRow.Key, row);
            }
        }

        private IScalar CreateScalar(string fieldType, string value)
        {
            switch(fieldType)
            {
                case "GlitchFinder.Scalars.Integer":
                    Scalars.Integer.TryParse(value, out var integer);
                    return integer;
                case "GlitchFinder.Scalars.Decimal":
                    Scalars.Decimal.TryParse(value, out var dec);
                    return dec;
                case "GlitchFinder.Scalars.DateTime":
                    Scalars.DateTime.TryParse(value, out var dateTime);
                    return dateTime;
                case "GlitchFinder.Scalars.String":
                    return new Scalars.String(value);
                default:
                    throw new Exception();
            }
        }
        #endregion
    }
}
