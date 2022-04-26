using System;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GlitchFinder.Contracts;
using GlitchFinder.Matrix.Contracts;

namespace GlitchFinder.Reporters
{
    public class ExcelReport : IGlitchReport
    {
        private SharedStringTablePart shareStringPart;

        //        int stringIndex = 1;

        public void CreateReport(string filePath, IMatrix matrix, bool isEqual)
        {
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                Create(filePath, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.
                GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Differences"
            };

            // Get the SharedStringTablePart. If it does not exist, create a new one.
            if (spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }

            var fields = matrix.Fields.Where(f => f.Name != "Key").ToList();
            var column = 1;
            foreach (var field in fields)
            {
                SetString(column, 1, worksheetPart, field.Name);
                column++;
            }

            uint row = 2;
            var keys = matrix.Keys.OrderBy(k => k).ToList();
            foreach (var key in keys)
            {
                if (!matrix.TryGetRow(key, out var dataRow))
                    throw new Exception("Inconsistent matrix");
                row++;

                column = 1;
                foreach (var field in fields)
                {
                    if (dataRow.TryGetValue(field.Name, out var value))
                    {
                        if(value is Scalars.DateTime)
                        {
                            SetDate(column, row, worksheetPart, ((Scalars.DateTime)value.Value).Value);
                        }
                        else if(value is Scalars.Integer)
                        {
                            SetNumber(column, row, worksheetPart, ((Scalars.Integer)value.Value).Value);
                        }
                        else if (value is Scalars.Decimal)
                        {
                            SetNumber(column, row, worksheetPart, ((Scalars.Decimal)value.Value).Value);
                        }
                        else
                        {
                            SetString(column, row, worksheetPart, value.Value.ToString());
                        }
                    }

                    column++;
                }
            }

            sheets.Append(sheet);

            workbookpart.Workbook.Save();

            spreadsheetDocument.Close();
        }

        private void SetDate(int column, uint row, WorksheetPart worksheetPart, DateTime value)
        {
            var cell = InsertCellInWorksheet(((char)(64 + column)).ToString(), row, worksheetPart);

            cell.CellValue = new CellValue(value.ToOADate().ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Date);
        }

        private void SetNumber(int column, uint row, WorksheetPart worksheetPart, decimal value)
        {
            var cell = InsertCellInWorksheet(((char)(64 + column)).ToString(), row, worksheetPart);

            cell.CellValue = new CellValue(value.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
        }


        private void SetString(int column, uint row, WorksheetPart worksheetPart, string stringValue)
        {
            int index = InsertSharedStringItem(stringValue, shareStringPart);
            
            var cell = InsertCellInWorksheet(((char)(64 + column)).ToString(), row, worksheetPart);

            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
        }

        private Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            if (shareStringPart.SharedStringTable == null)
                shareStringPart.SharedStringTable = new SharedStringTable();

            int i = 0;

            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                    return i;

                i++;
            }

            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        public void NonUniqueKeys(string filePath, IMatrix leftMatrix, IMatrix rightMatrix, bool isEqual)
        {
            throw new NotImplementedException("Excel report for non-unique keys is not implemented");
        }
    }
}
