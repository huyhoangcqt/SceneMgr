using System.Collections.Generic;
using System.IO;

namespace Snowwolf
{
    /// <summary>
    /// Transformed cell data from excel cell.
    /// </summary>
    public class DataCell
    {
        public int excelRow;
        public int excelColumn;

        public DataCellValue value = new DataCellValue();

        public void Clear()
        {
            excelRow = 0;
            excelColumn = 0;
            value.Clear();
        }
    }
}