using System.Collections.Generic;

namespace Snowwolf
{
    /// <summary>
    /// Transformed sheet from ExcelWorkSheet.
    /// </summary>
    public class DataSheet
    {
        public string sheetName;
        public int startExcelRow;
        public int endExcelRow;
        public int endExcelColumn;

        private SheetHeader m_Header = new SheetHeader();
        private List<DataCell> m_Cells = new List<DataCell>();

        public SheetHeader header => m_Header;

        public int columns => m_Header.items.Count;

        public int rows => columns == 0 ? 0 : (m_Cells.Count / columns);

        public DataCell this[int row, int column]
        {
            get
            {
                if (column > this.columns || column < 1)
                {
                    throw new System.ArgumentOutOfRangeException("column", column, "value out of range.");
                }
                if (row > rows || row < 1)
                {
                    throw new System.ArgumentOutOfRangeException("row", row, "value out of range.");
                }
                return m_Cells[(row - 1) * columns + (column - 1)];
            }
        }

        public void AddRow(List<DataCell> listToGetRows = null)
        {
            if (columns == 0)
            {
                throw new System.InvalidOperationException("You should set header first.");
            }
            listToGetRows?.Clear();
            for (int i = 0, cnt = columns; i < cnt; ++i)
            {
                DataCell newCell = new DataCell();
                m_Cells.Add(newCell);
                listToGetRows?.Add(newCell);
            }
        }

        public void Reset()
        {
            sheetName = null;
            m_Header.Clear();
            m_Cells.Clear();
        }
    }
}
