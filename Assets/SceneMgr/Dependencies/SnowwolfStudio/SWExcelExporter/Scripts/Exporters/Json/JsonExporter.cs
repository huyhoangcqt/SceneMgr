using System.IO;
using System.Text;
using LitJson;
using UnityEngine;

namespace Snowwolf
{    
    public class JsonExporter : Exporter
    {
        public override string name => "Json";

        public override string displayName => "Json";

        public static bool prettyPrint = true;

        public override void Export(string targetPath, DataSheet dataSheet, bool forServer)
        {
            using(StringWriter sw = new StringWriter())
            {
                sw.NewLine = "\n";
                WriteSheet(dataSheet, sw, forServer);
                byte[] bytes = Encoding.UTF8.GetBytes(sw.ToString());
                string exportPath = Path.Combine(targetPath, dataSheet.sheetName + ".json");
                FileUtils.ResolveFileDirectory(exportPath);
                File.WriteAllBytes(exportPath, bytes);
            }
        }

        public static void WriteSheet(DataSheet dataSheet, TextWriter writer, bool forServer)
        {
            JsonData jsonData = DataSheetToJson(dataSheet, forServer);
            JsonWriter jsonWriter = new JsonWriter(writer);
            jsonWriter.PrettyPrint = prettyPrint;
            jsonData.ToJson(jsonWriter);
        }

        private static JsonData DataSheetToJson(DataSheet dataSheet, bool forServer)
        {
            JsonData sheetJsonData = new JsonData();
            string sheetName = dataSheet.sheetName;
            var headerItems = dataSheet.header.items;

            //write header
            JsonData headerData = new JsonData();
            sheetJsonData["__headers__"] = headerData;
            for (int i = 0, cnt = headerItems.Count; i < cnt; ++i)
            {
                var headerItem = headerItems[i];
                if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }
                headerData[headerItem.name] = headerItem.GetValueTypeName();
            }

            //write rows
            for (int i = 0, cnt= dataSheet.rows; i < cnt; ++i)
            {
                JsonData rowData = new JsonData();
                for (int j = 0, cnt2 = dataSheet.columns; j < cnt2; ++j)
                {
                    var headerItem = headerItems[j];
                    if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }

                    DataCell dataCell = dataSheet[i + 1, j+ 1];

                    if (j == 0)
                    {
                        //Key
                        if (dataCell.value.valueType != CellValueType.String)
                        {
                            throw new System.NotSupportedException(
                                string.Format("[JsonExporter]Key used in JSON must be type of string. Error occurs in Cell[{0}, {1}].", i + 1, j + 1));
                        }
                        sheetJsonData[dataCell.value.strValue] = rowData;
                    }

                    string cellkey = headerItem.name;
                    if (headerItem.valueType == CellValueType.LocalizedString)
                    {
                        cellkey = localizedStringPrefix + cellkey;
                    }
                    rowData[cellkey] = CellValueToJsonData(dataCell.value);
                }
            }
            //<---Data sheet end
            return sheetJsonData;
        }

        private static JsonData CellValueToJsonData(DataCellValue dataCellValue)
        {
            JsonData jsonData = null;
            switch(dataCellValue.valueType)
            {
                case CellValueType.Byte:
                case CellValueType.Int:
                    jsonData = new JsonData((int)dataCellValue.numValue);
                    break;
                case CellValueType.Long:
                    jsonData = new JsonData(dataCellValue.numValue);
                    break;
                case CellValueType.UInt:
                    jsonData = new JsonData((long)dataCellValue.unumValue);
                    break;
                case CellValueType.ULong:
                    jsonData = new JsonData((long)dataCellValue.unumValue);
                    break;
                case CellValueType.Float:
                    jsonData = new JsonData(dataCellValue.floatValue);
                    break;
                case CellValueType.Double:
                    jsonData = new JsonData(dataCellValue.doubleValue);
                    break;
                case CellValueType.String:
                case CellValueType.LocalizedString:
                    {
                        // string strToWrite = CellValueConvert.MemoryStringToFile(dataCellValue.strValue);
                        jsonData = new JsonData(dataCellValue.strValue);
                    }
                    break;
                case CellValueType.Array:
                    {
                        jsonData = new JsonData();
                        jsonData.SetJsonType(JsonType.Array);
                        int length = dataCellValue.arrayValue.Count;
                        for(int i = 0; i < length; ++i)
                        {
                            jsonData.Add(CellValueToJsonData(dataCellValue.arrayValue[i]));
                        }
                    }
                    break;
                default:
                    throw new System.NotImplementedException(dataCellValue.valueType.ToString());
            }
            return jsonData;
        }
    }
}

