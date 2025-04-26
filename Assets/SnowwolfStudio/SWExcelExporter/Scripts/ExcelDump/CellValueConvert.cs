using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace Snowwolf
{
    /// <summary>
    /// Utilities to convert CellValue among Excel, memory and exported file.
    /// </summary>
    public static class CellValueConvert
    {
        private static StringBuilder s_StringBuilder = new StringBuilder(128);

        /// <summary>
        /// The separator for string array. We do not use (,) for string array because of the complicated splitting problem.
        /// </summary>
        public static char stringArraySeparator = '|';

        /// <summary>
        /// Convert excel cell value to readable string, value starts and ends with (") will be treated as program string.
        /// </summary>
        public static string ExcelCellValueToString(ExcelRange cell)
        {
            object value = cell.Value;
            string strValue = value != null ? value.ToString().Trim() : "";
            return strValue;
        }

        public static string ExcelStringToMemory(string value)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            s_StringBuilder.Clear();
            for (int i = 0, cnt = value.Length; i < cnt; ++i)
            {
                char c = value[i];
                switch(c)
                {
                    case '"':
                        if ((i == 0 && value[cnt - 1] == '"') || (i == (cnt - 1) && value[0] == '"')) { break; }
                        s_StringBuilder.Append(c);
                        break;
                    case '\r':
                        break;
                    case '\\':
                        {
                            if (i == cnt - 1)
                            {
                                s_StringBuilder.Append(c);
                                break;
                            }
                            ++i;
                            c = value[i];
                            if (c == 'r')
                            {
                                break;
                            }
                            else if (c == 'n')
                            {
                                s_StringBuilder.Append('\n');
                            }
                            else if (c == '"')
                            {
                                s_StringBuilder.Append('"');
                            }
                            else
                            {
                                s_StringBuilder.Append('\\');
                                s_StringBuilder.Append(c);
                            }
                        }
                        break;
                    default:
                        s_StringBuilder.Append(c);
                        break;
                }
            }
            return s_StringBuilder.ToString();
        }

        public static string MemoryStringToFile(string value)
        {
            if (string.IsNullOrEmpty(value)) { return ""; }
            
            s_StringBuilder.Clear();
            bool escaped = false;
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\"':
                        s_StringBuilder.Append("\\\"");
                        escaped = true;
                        break;
                    case '\n':
                        s_StringBuilder.Append("\\n");
                        escaped = true;
                        break;
                    default:
                        s_StringBuilder.Append(c);
                        break;
                }
            }
            return escaped ? s_StringBuilder.ToString() : value;
        }

        public static bool IsStringValueType(CellValueType valueType)
        {
            return (valueType == CellValueType.String) ||
                    (valueType == CellValueType.LocalizedString);
        }

        public static void SetDataCellValue(DataCellValue celValue, string valStr)
        {
            if (string.IsNullOrEmpty(valStr) && (celValue.valueType <= CellValueType.Double))
            {
                valStr = "0";
            }
            switch (celValue.valueType)
            {
                case CellValueType.Byte:
                    celValue.numValue = System.Convert.ToByte(valStr);
                    break;
                case CellValueType.Int:
                    celValue.numValue = System.Convert.ToInt32(valStr);
                    break;
                case CellValueType.UInt:
                    celValue.unumValue = System.Convert.ToUInt32(valStr);
                    break;
                case CellValueType.Long:
                    celValue.numValue = System.Convert.ToInt64(valStr);
                    break;
                case CellValueType.ULong:
                    celValue.unumValue = System.Convert.ToUInt64(valStr);
                    break;
                case CellValueType.Float:
                    celValue.floatValue = System.Convert.ToSingle(valStr);
                    break;
                case CellValueType.Double:
                    celValue.doubleValue = System.Convert.ToDouble(valStr);
                    break;
                case CellValueType.String:
                case CellValueType.LocalizedString:
                    {
                        celValue.strValue = ExcelStringToMemory(valStr);
                    }
                    break;
                case CellValueType.Array:
                    {
                        string[] splitValues = valStr.Split(IsStringValueType(celValue.arrayElementType) ? stringArraySeparator : ',');
                        var array = celValue.GetUsableCleanArray();
                        for (int i = 0, cnt = splitValues.Length; i < cnt; ++i)
                        {
                            string itemValue = splitValues[i].Trim();
                            DataCellValue newValue = new DataCellValue(celValue.arrayElementType);
                            SetDataCellValue(newValue, itemValue);
                            array.Add(newValue);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public static void SetDataCellValue(DataCell cell, string valStr)
        {
            SetDataCellValue(cell.value, valStr);
        }
    }
}