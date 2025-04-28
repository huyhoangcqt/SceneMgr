namespace Snowwolf
{
    /// <summary>
    /// Convert cell value types between Excel and memory.
    /// </summary>
    public static class TypeConvert
    {
        public static CellValueType ExcelToCustom(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) { return CellValueType.Invalid; }
            if (typeStr.EndsWith("[]"))
            {
                string elementTypeStr = typeStr.Substring(0, typeStr.Length - 2);
                CellValueType elementType = ExcelToCustom(elementTypeStr);
                return elementType != CellValueType.Invalid ? CellValueType.Array : CellValueType.Invalid;
            }

            CellValueType cellValueType;
            switch (typeStr)
            {
                case "byte":
                    cellValueType = CellValueType.Byte;
                    break;
                case "int":
                    cellValueType = CellValueType.Int;
                    break;
                case "uint":
                    cellValueType = CellValueType.UInt;
                    break;
                case "long":
                    cellValueType = CellValueType.Long;
                    break;
                case "ulong":
                    cellValueType = CellValueType.ULong;
                    break;
                case "float":
                    cellValueType = CellValueType.Float;
                    break;
                case "double":
                    cellValueType = CellValueType.Double;
                    break;
                case "string":
                    cellValueType = CellValueType.String;
                    break;
                case "localizedstring":
                    cellValueType = CellValueType.LocalizedString;
                    break;
                default:
                    cellValueType = CellValueType.Invalid;
                    break;
            }

            return cellValueType;
        }

        public static CellValueType GetArrayElementType(string typeStr)
        {
            string elementTypeStr = typeStr.Substring(0, typeStr.Length - 2);
            return ExcelToCustom(elementTypeStr);
        }
    }
}