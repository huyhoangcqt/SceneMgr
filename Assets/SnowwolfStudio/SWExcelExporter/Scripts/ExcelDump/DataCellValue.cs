using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowwolf
{
    /// <summary>
    /// Keep real cell value from excel string item.
    /// </summary>
    public class DataCellValue
    {
        public CellValueType valueType;
        public CellValueType arrayElementType;
        public string strValue;
        public long numValue;
        public ulong unumValue;
        public float floatValue;
        public double doubleValue;

        public List<DataCellValue> arrayValue;

        public DataCellValue(CellValueType valType = CellValueType.Invalid, CellValueType arrElementType = CellValueType.Invalid)
        {
            valueType = valType;
            arrayElementType = arrElementType;
        }

        public List<DataCellValue> GetUsableCleanArray()
        {
            if (arrayValue == null)
            {
                arrayValue = new List<DataCellValue>();
            }
            arrayValue.Clear();
            return arrayValue;
        }

        public void Clear()
        {
            valueType = CellValueType.Invalid;
            strValue = null;
            numValue = 0;
            unumValue = 0;
            floatValue = 0;
            doubleValue = 0;
            arrayValue = null;
        }
    }
}
