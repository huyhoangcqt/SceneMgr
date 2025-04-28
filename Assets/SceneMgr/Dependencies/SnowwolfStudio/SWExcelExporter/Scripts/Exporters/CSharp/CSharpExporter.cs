using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace Snowwolf
{    
    public class CSharpExporter : Exporter
    {
        public override string name => "CSharp";

        public override string displayName => "C#+Binary";

        public const string CSharpScriptFolder = "Scripts/ExcelData";

        public const string CSharpDataServiceFolder = "Scripts/DataService";

        public const string CSharpDataFolder = "Resources/ExcelData";

        private const string k_CSharpFileTemplate = @"using System.IO;
using System.Collections.Generic;

namespace ExcelData
{{
    public class {0} : {1}
    {{
        public class Item
        {{
{2}
        }}

        private static {0} s_Instance;
        private static {0} Instance
        {{
            get
            {{
                if (s_Instance == null)
                {{
                    s_Instance = new {0}();
                    s_Instance.Init();
                    DataService.RegisterSheet(s_Instance);
                }}
                return s_Instance;
            }}
        }}

        public static Item GetItem({3} key)
        {{
            Instance.m_Items.TryGetValue(key, out Item foundItem);
            #if UNITY_EDITOR
            if (foundItem == null)
            {{
                UnityEngine.Debug.LogWarningFormat(""{{0}} do not contains item of key '{{1}}'."", Instance.sheetName, key);
            }}
            #endif
            return foundItem;
        }}

        public static IEnumerable<KeyValuePair<{3}, Item>> GetDict()
        {{
            return Instance.m_Items;
        }}

        private Dictionary<{3}, Item> m_Items = new Dictionary<{3}, Item>();

        public string sheetName => ""{0}"";

        private void Init()
        {{
            byte[] bytes = DataService.GetSheetBytes(sheetName);
            using (MemoryStream ms = new MemoryStream(bytes))
            {{
                using(BinaryReader reader = new BinaryReader(ms))
                {{
                    reader.ReadString(); //sheetName

                    //Read header
                    SheetHeader sheetHeader = new SheetHeader();
                    sheetHeader.ReadFrom(reader);
                    List<SheetHeader.Item> headerItems = sheetHeader.items;

                    int columns = headerItems.Count;
                    int rows = reader.ReadInt32();

                    //Get Item indices
{4}

                    #if UNITY_EDITOR
                    bool promptMismatchColumns = false;
                    #endif
                    for (int i = 0; i < rows; ++i)
                    {{
                        Item newItem = new Item();
                        for (int j = 0; j < columns; ++j)
                        {{
                            SheetHeader.Item headerItem = headerItems[j];

{5}
                            else
                            {{
                                DataService.ReadAndDrop(reader, headerItem.valType);
                                #if UNITY_EDITOR
                                if (!promptMismatchColumns)
                                {{
                                    UnityEngine.Debug.LogWarningFormat(""Data sheet '{{0}}' find mismatch columns for '{{1}}({{2}})'."", sheetName, headerItem.name, headerItem.valType);
                                    promptMismatchColumns = true;
                                }}
                                #endif
                            }}
                        }}
                        m_Items.Add(newItem.{6}, newItem);
                    }}
                }}
            }}
            {7}
        }}
{8}
    }}
}}
    ";

        public override void Export(string targetPath, DataSheet dataSheet, bool forServer)
        {
            //Export C# script.
            using(StringWriter sw = new StringWriter())
            {
                sw.NewLine = "\n";
                WriteCS(dataSheet, sw, forServer);
                byte[] bytes = Encoding.UTF8.GetBytes(sw.ToString());
                string csPath = Path.Combine(targetPath, CSharpScriptFolder, dataSheet.sheetName + ".cs");
                FileUtils.ResolveFileDirectory(csPath);
                File.WriteAllBytes(csPath, bytes);
            }

            //Export binary data.
            string dataPath = Path.Combine(targetPath, CSharpDataFolder, dataSheet.sheetName + ".bytes");
            FileUtils.ResolveFileDirectory(dataPath);
            using(FileStream fs = new FileStream(dataPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using(BinaryWriter binaryWriter = new BinaryWriter(fs))
                {
                    WriteData(dataSheet, binaryWriter, forServer);
                }
            }

            //Export DataService.cs File
            byte[] dataServiceBytes = Encoding.UTF8.GetBytes(DataServiceTemplate.dataServiceFileCode);
            string dataServicePath = Path.Combine(targetPath, CSharpDataServiceFolder, "DataService.cs");
            FileUtils.ResolveFileDirectory(dataServicePath);
            File.WriteAllBytes(dataServicePath, dataServiceBytes);
        }

        public static string CustomToCSharpTypeStr(SheetHeader.Item headItem)
        {
            string typeStr;
            CellValueType testValueType = headItem.valueType == CellValueType.Array ? headItem.arrayElementType : headItem.valueType;
            switch(testValueType)
            {
                case CellValueType.Byte:
                case CellValueType.Int:
                case CellValueType.UInt:
                case CellValueType.Long:
                case CellValueType.ULong:
                case CellValueType.Float:
                case CellValueType.Double:
                    typeStr = testValueType.ToString().ToLower();
                    break;
                case CellValueType.String:
                case CellValueType.LocalizedString:
                    typeStr = "string";
                    break;
                default:
                    throw new System.NotImplementedException(headItem.valueType.ToString());
            }
            if (headItem.valueType == CellValueType.Array)
            {
                typeStr += "[]";
            }

            return typeStr;
        }

        public static void WriteCS(DataSheet dataSheet, TextWriter writer, bool forServer)
        {
            string sheetName = dataSheet.sheetName;
            StringBuilder stringBuilder = new StringBuilder();
            var headerItems = dataSheet.header.items;

            string baseType = dataSheet.header.ContainsLocalizedString() ? "ILocalizationSheet":"IDataSheet";

            //Item definition
            stringBuilder.Clear();
            for (int i = 0, cnt = headerItems.Count; i < cnt; ++i)
            {
                var headerItem = headerItems[i];
                if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }
                string typeStr = CustomToCSharpTypeStr(headerItem);
                if (headerItem.valueType == CellValueType.LocalizedString)
                {
                    stringBuilder.AppendFormat("            public {0} {1}{2};\n", typeStr, localizedStringPrefix, headerItem.name);
                }
                stringBuilder.AppendFormat("            public {0} {1};", typeStr, headerItem.name);
                if (i != cnt - 1){ stringBuilder.Append('\n');}
            }
            string itemDefinition = stringBuilder.ToString();

            string keyTypeStr = CustomToCSharpTypeStr(headerItems[0]);

            //Item indices
            stringBuilder.Clear();
            for (int i = 0, cnt = headerItems.Count; i < cnt; ++i)
            {
                var headerItem = headerItems[i];
                if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }

                stringBuilder.Append("                    ");
                stringBuilder.AppendFormat("int {0}Index = sheetHeader.IndexOf(\"{0}\", \"{1}\");", headerItem.name, headerItem.GetValueTypeName());
                if (i != cnt - 1){ stringBuilder.Append('\n');}
            }
            string itemIndices = stringBuilder.ToString();

            //Item reading
            stringBuilder.Clear();
            for (int i = 0, cnt = headerItems.Count; i < cnt; ++i)
            {
                var headerItem = headerItems[i];
                if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }

                stringBuilder.Append("                            ");
                if (i == 0)
                {
                    stringBuilder.AppendFormat("if (j == {0}Index)\n", headerItem.name);
                }
                else
                {
                    stringBuilder.AppendFormat("else if (j == {0}Index)\n", headerItem.name);
                }
                stringBuilder.Append("                            {\n");
                GetItemReadText(stringBuilder, headerItem, i);
                stringBuilder.Append("                            }");
                if (i != cnt - 1){ stringBuilder.Append('\n');}
            }
            string itemReading = stringBuilder.ToString();

            string initLocalStr = "";
            string localizationFunc = "";
            if (dataSheet.header.ContainsLocalizedString())
            {
                initLocalStr = "\n            RefreshLocalizationValues();";

                stringBuilder.Clear();
                stringBuilder.Append("\n        public void RefreshLocalizationValues()\n");
                stringBuilder.Append("        {\n");
                stringBuilder.Append("            foreach(var kv in m_Items)\n");
                stringBuilder.Append("            {\n");
                stringBuilder.Append("                Item item = kv.Value;\n");
                for (int i = 0, cnt = headerItems.Count; i < cnt; ++i)
                {
                    var headerItem = headerItems[i];
                    if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }
                    if (headerItem.valueType != CellValueType.LocalizedString) { continue; }
                    stringBuilder.Append("                ");
                    stringBuilder.AppendFormat("item.{0} = DataService.GetLocalizedText(sheetName, item.{1}{0});\n", 
                        headerItem.name, localizedStringPrefix);
                }
                stringBuilder.Append("            }\n");
                stringBuilder.Append("        }");
                localizationFunc = stringBuilder.ToString();
            }

            //Final text
            stringBuilder.Clear();
            stringBuilder.AppendFormat(k_CSharpFileTemplate, sheetName, baseType, itemDefinition,
                keyTypeStr, itemIndices, itemReading, headerItems[0].name, initLocalStr, localizationFunc);
            writer.Write(stringBuilder.ToString());
        }

        private static void WriteHeader(SheetHeader header, BinaryWriter writer, bool forServer)
        {
            List<SheetHeader.Item> items = header.items;
            int itemCount = items.Count;

            int excludedCount = 0;
            for (int i = 0; i < itemCount; ++i)
            {
                SheetHeader.Item headerItem = items[i];
                if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { ++excludedCount; }
            }

            writer.Write(itemCount - excludedCount);
            for (int i = 0; i < itemCount; ++i)
            {
                SheetHeader.Item headerItem = items[i];
                if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }

                writer.Write(headerItem.name ?? "");
                writer.Write(headerItem.GetValueTypeName());
            }
        }

        private static void WriteCellValue(DataCellValue dataCellValue, BinaryWriter writer)
        {
            switch(dataCellValue.valueType)
            {
                case CellValueType.Byte:
                    writer.Write((byte)dataCellValue.numValue);
                    break;
                case CellValueType.Int:
                    writer.Write((int)dataCellValue.numValue);
                    break;
                case CellValueType.Long:
                    writer.Write((long)dataCellValue.numValue);
                    break;
                case CellValueType.UInt:
                    writer.Write((uint)dataCellValue.unumValue);
                    break;
                case CellValueType.ULong:
                    writer.Write((ulong)dataCellValue.unumValue);
                    break;
                case CellValueType.Float:
                    writer.Write(dataCellValue.floatValue);
                    break;
                case CellValueType.Double:
                    writer.Write(dataCellValue.doubleValue);
                    break;
                case CellValueType.String:
                    writer.Write(dataCellValue.strValue ?? "");
                    break;
                case CellValueType.LocalizedString:
                    writer.Write(dataCellValue.strValue ?? "");
                    break;
                case CellValueType.Array:
                    {
                        writer.Write(dataCellValue.arrayValue.Count);
                        for(int i = 0, cnt = dataCellValue.arrayValue.Count; i < cnt; ++i)
                        {
                            WriteCellValue(dataCellValue.arrayValue[i], writer);
                        }
                    }
                    break;
                default:
                    throw new System.NotImplementedException(dataCellValue.valueType.ToString());
            }
        }

        private static void WriteCellData(DataCell dataCell, BinaryWriter writer)
        {
            WriteCellValue(dataCell.value, writer);
        }

        public static void WriteData(DataSheet dataSheet, BinaryWriter writer, bool forServer)
        {
            writer.Write(dataSheet.sheetName ?? "");
            WriteHeader(dataSheet.header, writer, forServer);
            writer.Write(dataSheet.rows);
            for (int i = 0, cnt= dataSheet.rows; i < cnt; ++i)
            {
                for (int j = 0, cnt2 = dataSheet.columns; j < cnt2; ++j)
                {
                    DataCell dataCell = dataSheet[i + 1, j+ 1];
                    SheetHeader.Item headerItem = dataSheet.header.items[j];
                    if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }
                    WriteCellData(dataCell, writer);
                }
            }
        }

        private static string GetCellValueReadText(CellValueType valueType)
        {
            string readText = null;
            switch(valueType)
            {
                case CellValueType.Byte:
                    readText = "reader.ReadByte()";
                    break;
                case CellValueType.Int:
                    readText = "reader.ReadInt32()";
                    break;
                case CellValueType.UInt:
                    readText = "reader.ReadUInt32()";
                    break;
                case CellValueType.Long:
                    readText = "reader.ReadInt64()";
                    break;
                case CellValueType.ULong:
                    readText = "reader.ReadUInt64()";
                    break;
                case CellValueType.Float:
                    readText = "reader.ReadSingle()";
                    break;
                case CellValueType.Double:
                    readText = "reader.ReadDouble()";
                    break;
                case CellValueType.String:
                    readText = "reader.ReadString()";
                    break;
                case CellValueType.LocalizedString:
                    readText = "reader.ReadString()";
                    break;
                default:
                    throw new System.NotImplementedException(valueType.ToString());
            }
            return readText;
        }

        private static string GetItemReadText(StringBuilder stringBuilder, SheetHeader.Item headerItem, int index)
        {
            stringBuilder.Append("                                ");
            if (headerItem.valueType == CellValueType.Array)
            {
                CellValueType arrayElementType = headerItem.arrayElementType;
                stringBuilder.AppendFormat("int l_{0} = reader.ReadInt32();\n", index);
                stringBuilder.AppendFormat("                                newItem.{0} = new {1}[l_{2}];\n", headerItem.name, arrayElementType.ToString().ToLower(), index);

                stringBuilder.Append("                                ");
                stringBuilder.AppendFormat("for(int i_{0} = 0; i_{0} < l_{0}; ++i_{0})\n", index);
                stringBuilder.Append("                                {\n");
                stringBuilder.Append("                                    ");
                stringBuilder.AppendFormat("newItem.{0}[i_{1}] = {2};\n", headerItem.name, index, GetCellValueReadText(arrayElementType));
                stringBuilder.Append("                                }\n");
            }
            else
            {
                switch(headerItem.valueType)
                {
                    case CellValueType.Byte:
                    case CellValueType.Int:
                    case CellValueType.UInt:
                    case CellValueType.Long:
                    case CellValueType.ULong:
                    case CellValueType.Float:
                    case CellValueType.Double:
                    case CellValueType.String:
                        stringBuilder.AppendFormat("newItem.{0} = {1};\n", headerItem.name, GetCellValueReadText(headerItem.valueType));
                        break;
                    case CellValueType.LocalizedString:
                        stringBuilder.AppendFormat("newItem.{0}{1} = {2};\n", localizedStringPrefix, headerItem.name, GetCellValueReadText(headerItem.valueType));
                        break;
                    default:
                        throw new System.NotImplementedException(headerItem.valueType.ToString());
                }
            }

            return stringBuilder.ToString();
        }
    }
}
