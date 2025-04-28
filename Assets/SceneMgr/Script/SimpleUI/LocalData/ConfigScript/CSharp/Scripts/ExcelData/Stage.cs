using System.IO;
using System.Collections.Generic;

namespace ExcelData
{
    public class Stage : IDataSheet
    {
        public class Item
        {
            public string key;
            public string name;
            public string chapter;
            public string lineup;
            public string map;
        }

        private static Stage s_Instance;
        private static Stage Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new Stage();
                    s_Instance.Init();
                    DataService.RegisterSheet(s_Instance);
                }
                return s_Instance;
            }
        }

        public static Item GetItem(string key)
        {
            Instance.m_Items.TryGetValue(key, out Item foundItem);
            #if UNITY_EDITOR
            if (foundItem == null)
            {
                UnityEngine.Debug.LogWarningFormat("{0} do not contains item of key '{1}'.", Instance.sheetName, key);
            }
            #endif
            return foundItem;
        }

        public static IEnumerable<KeyValuePair<string, Item>> GetDict()
        {
            return Instance.m_Items;
        }

        private Dictionary<string, Item> m_Items = new Dictionary<string, Item>();

        public string sheetName => "Stage";

        private void Init()
        {
            byte[] bytes = DataService.GetSheetBytes(sheetName);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using(BinaryReader reader = new BinaryReader(ms))
                {
                    reader.ReadString(); //sheetName

                    //Read header
                    SheetHeader sheetHeader = new SheetHeader();
                    sheetHeader.ReadFrom(reader);
                    List<SheetHeader.Item> headerItems = sheetHeader.items;

                    int columns = headerItems.Count;
                    int rows = reader.ReadInt32();

                    //Get Item indices
                    int keyIndex = sheetHeader.IndexOf("key", "string");
                    int nameIndex = sheetHeader.IndexOf("name", "string");
                    int chapterIndex = sheetHeader.IndexOf("chapter", "string");
                    int lineupIndex = sheetHeader.IndexOf("lineup", "string");
                    int mapIndex = sheetHeader.IndexOf("map", "string");

                    #if UNITY_EDITOR
                    bool promptMismatchColumns = false;
                    #endif
                    for (int i = 0; i < rows; ++i)
                    {
                        Item newItem = new Item();
                        for (int j = 0; j < columns; ++j)
                        {
                            SheetHeader.Item headerItem = headerItems[j];

                            if (j == keyIndex)
                            {
                                newItem.key = reader.ReadString();
                            }
                            else if (j == nameIndex)
                            {
                                newItem.name = reader.ReadString();
                            }
                            else if (j == chapterIndex)
                            {
                                newItem.chapter = reader.ReadString();
                            }
                            else if (j == lineupIndex)
                            {
                                newItem.lineup = reader.ReadString();
                            }
                            else if (j == mapIndex)
                            {
                                newItem.map = reader.ReadString();
                            }
                            else
                            {
                                DataService.ReadAndDrop(reader, headerItem.valType);
                                #if UNITY_EDITOR
                                if (!promptMismatchColumns)
                                {
                                    UnityEngine.Debug.LogWarningFormat("Data sheet '{0}' find mismatch columns for '{1}({2})'.", sheetName, headerItem.name, headerItem.valType);
                                    promptMismatchColumns = true;
                                }
                                #endif
                            }
                        }
                        m_Items.Add(newItem.key, newItem);
                    }
                }
            }
            
        }

    }
}
    