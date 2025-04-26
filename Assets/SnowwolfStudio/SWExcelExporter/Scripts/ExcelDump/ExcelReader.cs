using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;

namespace Snowwolf
{
    /// <summary>
    /// Reader can be used to read OfficeOpenXml.ExcelWorkSheet to readable DataSheet.
    /// </summary>
    public class ExcelReader
    {
        public const int findStartMarkMaxRow = 100;

        /// <summary>
        /// Transform ExcelWorkSheet to readable DataSheet.
        /// </summary>
        public static DataSheet TransformDataSheet(ExcelWorksheet worksheet)
        {
            ExcelRange worksheetCells = worksheet.Cells;

            int rowIndex = 1;
            int startRow = -1;
            int excelRowMax = worksheetCells.Rows;
            int excelColumnMax = worksheetCells.Columns;

            if (excelRowMax < 1 || excelColumnMax < 1) { return null; }

            //Find start mark
            for (int maxRow = Mathf.Min(findStartMarkMaxRow, excelRowMax); rowIndex <= maxRow; ++rowIndex)
            {
                ExcelRange cell = worksheetCells[rowIndex, 1];
                if (ExcelReadMarks.payloadStartMark == CellValueConvert.ExcelCellValueToString(cell))
                {
                    startRow = rowIndex;
                    break;
                }
            }
            if ((startRow == -1) || (startRow + 3 > excelRowMax)) { return null; } //Not a valid worksheet.

            DataSheet dataSheet = new DataSheet();
            dataSheet.sheetName = worksheet.Name;
            dataSheet.startExcelRow = startRow;

            //Parse Header
            rowIndex = startRow + 1;
            for (int i = 1; i <= excelColumnMax; ++i)
            {
                ExcelRange excelTitleCell = worksheetCells[rowIndex, i];
                string title = CellValueConvert.ExcelCellValueToString(excelTitleCell);
                if (string.IsNullOrEmpty(title)) { break; }
                if (title.StartsWith(ExcelReadMarks.commentMark))
                {
                    //first column is key, which can not be commented.
                    if (i == 1){ break; }
                    continue;
                }

                ExcelRange excelTypeCell = worksheetCells[rowIndex + 1, i];
                string excelTypeStr = CellValueConvert.ExcelCellValueToString(excelTypeCell);
                CellValueType cellValType = TypeConvert.ExcelToCustom(excelTypeStr);
                if (cellValType == CellValueType.Invalid)
                {
                    break;
                }
                CellValueType arrayElementType = cellValType == CellValueType.Array ? TypeConvert.GetArrayElementType(excelTypeStr) : CellValueType.Invalid;
                if (arrayElementType == CellValueType.Array)
                {
                    throw new System.NotSupportedException(string.Format("Multidimensional array is not supported. Error occurs in cell [{0}].", excelTypeCell.Address));
                }

                //process server or client exclusive type
                SheetHeader.ItemExclusiveType exclusiveType = SheetHeader.ItemExclusiveType.Both;
                if (title.StartsWith(ExcelReadMarks.headerClientOnly))
                {
                    exclusiveType = SheetHeader.ItemExclusiveType.ClientOnly;
                    title = title.Substring(ExcelReadMarks.headerClientOnly.Length);
                }
                else if (title.StartsWith(ExcelReadMarks.headerServerOnly))
                {
                    exclusiveType = SheetHeader.ItemExclusiveType.ServerOnly;
                    title = title.Substring(ExcelReadMarks.headerServerOnly.Length);
                }

                SheetHeader.Item headItem = new SheetHeader.Item();
                headItem.name = title;
                headItem.valueType = cellValType;
                headItem.arrayElementType = arrayElementType;
                headItem.excelColumn = i;
                headItem.exclusiveType = exclusiveType;
                dataSheet.header.items.Add(headItem);
            }
            if (dataSheet.header.items.Count == 0) { return null; } //Invalid header

            dataSheet.endExcelColumn = dataSheet.header.items[dataSheet.header.items.Count - 1].excelColumn;

            //Parse Cells
            rowIndex = startRow + 2;
            List<DataCell> cellsOfRow = new List<DataCell>();
            while (++rowIndex <= excelRowMax)
            {
                string firstItem = CellValueConvert.ExcelCellValueToString(worksheetCells[rowIndex, 1]);

                if (string.IsNullOrEmpty(firstItem) || (firstItem.StartsWith(ExcelReadMarks.commentMark))) { continue; }
                if (firstItem == ExcelReadMarks.payloadStopMark) { break; }

                dataSheet.AddRow(cellsOfRow);
                for (int i = 0, cnt = dataSheet.columns; i < cnt; ++i)
                {
                    SheetHeader.Item headItem = dataSheet.header.items[i];
                    DataCell dataCell = cellsOfRow[i];
                    dataCell.value.valueType = headItem.valueType;
                    dataCell.value.arrayElementType = headItem.arrayElementType;
                    dataCell.excelColumn = headItem.excelColumn;
                    dataCell.excelRow = rowIndex;

                    ExcelRange excelCel = worksheetCells[rowIndex, headItem.excelColumn];
                    string cellValStr = headItem.excelColumn == 1 ? firstItem : CellValueConvert.ExcelCellValueToString(excelCel);
                    try
                    {
                        CellValueConvert.SetDataCellValue(dataCell, cellValStr);
                    }
                    catch(System.Exception e)
                    {
                        string errorMessage = string.Format("Error occur in cell[{0}], error: {1}", excelCel.Address, e.ToString());
                        throw new System.InvalidCastException(errorMessage);
                    }
                }
            }
            dataSheet.endExcelRow = rowIndex;

            return dataSheet;
        }
    }
}