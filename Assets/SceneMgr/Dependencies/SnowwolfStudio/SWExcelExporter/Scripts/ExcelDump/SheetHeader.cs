using System.Collections.Generic;

namespace Snowwolf
{
    /// <summary>
    /// Header data of DataSheet.
    /// </summary>
    public class SheetHeader
    {
        /// <summary>
        /// Indicate wheter the item works on server or client only.
        /// </summary>
        public enum ItemExclusiveType
        {
            ServerOnly,
            ClientOnly,
            Both
        }

        public struct Item
        {
            public string name;
            public CellValueType valueType;
            public CellValueType arrayElementType;
            public int excelColumn;
            public ItemExclusiveType exclusiveType;

            public string GetValueTypeName()
            {
                if (valueType == CellValueType.Invalid) { return "[Invalid]"; };
                return valueType == CellValueType.Array ? arrayElementType.ToString().ToLower() + "[]" : valueType.ToString().ToLower();
            }
        }

        public readonly List<Item> items = new List<Item>();

        public int IndexOf(string itemName)
        {
            for (int i = 0, cnt = items.Count; i < cnt; ++i)
            {
                Item item = items[i];
                if (item.name == itemName)
                {
                    return i;
                }
            }
            return -1;
        }

        public int IndexOf(int excelColumn)
        {
            for (int i = 0, cnt = items.Count; i < cnt; ++i)
            {
                Item item = items[i];
                if (item.excelColumn == excelColumn)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool ContainsLocalizedString()
        {
            int itemCount = items.Count;
            for (int i = 0; i < itemCount; ++i)
            {
                Item item = items[i];
                if (item.valueType == CellValueType.LocalizedString)
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            items.Clear();
        }
    }
}