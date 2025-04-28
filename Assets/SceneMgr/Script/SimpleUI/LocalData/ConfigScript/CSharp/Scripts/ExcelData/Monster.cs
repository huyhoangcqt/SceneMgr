using System.IO;
using System.Collections.Generic;

namespace ExcelData
{
    public class Monster : IDataSheet
    {
        public class Item
        {
            public string key;
            public string name;
            public string prefab;
        }

        private static Monster s_Instance;
        private static Monster Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new Monster();
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

        public string sheetName => "Monster";

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
                    int prefabIndex = sheetHeader.IndexOf("prefab", "string");

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
                            else if (j == prefabIndex)
                            {
                                newItem.prefab = reader.ReadString();
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
    