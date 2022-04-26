using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GlitchFinder.Matrix.Contracts;
using GlitchFinder.Matrix.DomainObjects;
using GlitchFinder.Scalars;

namespace GlitchFinder.Matrix
{
    public class MatrixCreator
    {
        private const string AddedKeyLabel = "_AddedKey";

        public static bool ParseMatrix(List<string> fieldNames, List<string> keyFieldNames, List<List<string>> rawMatrix, out IMatrix matrix)
        {

            var internalMatrix = DetectFieldTypes(fieldNames, rawMatrix, out var fields);

            bool addKey = false;
            int keyCounter = 1;
            if (keyFieldNames == null || keyFieldNames.Count == 0)
            {
                addKey = true;
                fieldNames.Add(AddedKeyLabel);
                fields.Add(new Field { Type = "GlitchFinder.Scalars.Integer", Name = AddedKeyLabel });
                keyFieldNames.Add(AddedKeyLabel);
            }
            
            var uniqueKeys = ParseUniqueKeyDefinition(keyFieldNames, fieldNames);

            // Get keys, check for uniqueness
            var workingMatrix = new Dictionary<string, List<Dictionary<string, IScalar>>>();
            var isKeysUnique = true;
            foreach (var row in internalMatrix)
            {
                if (addKey)
                    row.Add(AddedKeyLabel, new Scalars.Integer(keyCounter++));

                var key = GetKey(uniqueKeys, row);
                if (workingMatrix.ContainsKey(key))
                    isKeysUnique = false;
                else
                    workingMatrix.Add(key, new List<Dictionary<string, IScalar>>());

                workingMatrix[key].Add(row);
            }

            matrix = new DomainObjects.Matrix();
            matrix.Fields = fields;
            if (isKeysUnique)
            {
                matrix.UniqueKeys = uniqueKeys;
                foreach (var kv in workingMatrix)
                {
                    var cellRow = kv.Value.Single().ToDictionary(k => k.Key, v => (ICell)new Cell(v.Value));
                    matrix.SetRow(kv.Key, cellRow);
                }
            }
            else
            {
                // Build matrix for non-unique keys
                var counter = 0;
                foreach (var kv in workingMatrix)
                {
                    if (kv.Value.Count > 1)
                    {
                        foreach (var row in kv.Value)
                        {
                            var cellRow = kv.Value.Single().ToDictionary(k => k.Key, v => (ICell)new Cell(v.Value));
                            matrix.SetRow(counter.ToString(), cellRow);
                            counter++;
                        }
                    }
                }
            }
            return isKeysUnique;
        }


        private static List<Dictionary<string, IScalar>> DetectFieldTypes(List<string> fieldNames, List<List<string>> rawMatrix, out List<Field> fields)
        {
            var internalMatrix = new List<Dictionary<string, IScalar>>();

            var c = 0;
            fields = new List<Field>();
            foreach (var fieldName in fieldNames)
            {
                bool successfullyParsedAllRows = false;
                foreach (var scalar in ScalarPrecedence.Precedence)
                {
                    successfullyParsedAllRows = true;
                    var allCells = new List<IScalar>();
                    foreach (var row in rawMatrix)
                    {
                        var value = row[c];
                        var parameters = new object[] { value, null };
                        var isOfType = (bool)scalar.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static).Invoke(null, parameters);
                        if (!isOfType)
                        {
                            // Parse failed so this column is of some other type
                            successfullyParsedAllRows = false;
                            break;
                        }
                        else
                        {
                            IScalar parsedScalar = (IScalar)parameters[1];
                            allCells.Add(parsedScalar);
                        }
                    }
                    if (successfullyParsedAllRows)
                    {
                        // Field Type established - every row was parseable to this type
                        fields.Add(new Field() { Name = fieldName, Type = scalar.ToString() });

                        var rowNo = 0;
                        foreach (var cell in allCells)
                        {
                            if (internalMatrix.Count <= rowNo)
                                internalMatrix.Add(new Dictionary<string, IScalar>());
                            internalMatrix[rowNo].Add(fieldName, cell);

                            rowNo++;
                        }
                        break;
                    }
                }
                c++;
                if (!successfullyParsedAllRows)
                    throw new Exception("This should never happen - everything should be castable to last type (string)");
            }
            return internalMatrix;
        }
        private static List<string> ParseUniqueKeyDefinition(List<string> uniqueKeyFields, List<string> fieldNames)
        {
            var validKeyFields = uniqueKeyFields.Intersect(fieldNames).ToList();

            if (validKeyFields.Count != uniqueKeyFields.Count)
                throw new Exception($"Couldn't find part of unique key fields ({string.Join(",", uniqueKeyFields)}) in list of fields ({string.Join(",", fieldNames)})");

            return validKeyFields;        
        }

        private static string GetKey(List<string> uniqueKey, Dictionary<string, IScalar> row)
        {
            var key = "";
            foreach (var keyPart in uniqueKey)
            {
                if (key == "")
                    key = ";";
                key += row[keyPart].ToString() + ";";
            }
            return key;
        }
    }
}
