using System.IO;
using System.Text;

namespace Snowwolf
{    
    public class LuaExporter : Exporter
    {
        public override string name => "Lua";

        public override string displayName => "Lua";

        public const string LuaDataFolder = "Lua/ExcelData";

        public override void Export(string targetPath, DataSheet dataSheet, bool forServer)
        {
            using(StringWriter sw = new StringWriter())
            {
                sw.NewLine = "\n";
                WriteSheet(dataSheet, sw, forServer);
                byte[] bytes = Encoding.UTF8.GetBytes(sw.ToString());
                string exportPath = Path.Combine(targetPath, LuaDataFolder, dataSheet.sheetName + ".lua");
                FileUtils.ResolveFileDirectory(exportPath);
                File.WriteAllBytes(exportPath, bytes);
            }
        }

        public static void WriteSheet(DataSheet dataSheet, TextWriter writer, bool forServer)
        {
            string sheetName = dataSheet.sheetName;

            writer.WriteLine("module(\"ExcelData\")");

            //TODO:Localization snippets, You may use your own version.
            writer.WriteLine("local fileName = \"{0}\"", sheetName);
            writer.WriteLine("local GetLocalizedText = DataService and DataService.GetLocalizedText");

            writer.WriteLine();

            //--->Data sheet start
            writer.WriteLine("{0}={{", dataSheet.sheetName);

            var headerItems = dataSheet.header.items;

            //write header
            writer.Write("    --[\"header\"]={");
            for (int i = 0, cnt = headerItems.Count; i < cnt; ++i)
            {
                var headerItem = headerItems[i];
                if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }
                
                writer.Write("{0}=\"{1}\"", headerItem.name, headerItem.GetValueTypeName());
                if (i != cnt - 1)
                {
                    writer.Write(",");
                }
            }
            writer.WriteLine("}");

            //write rows
            for (int i = 0, cnt= dataSheet.rows; i < cnt; ++i)
            {
                for (int j = 0, cnt2 = dataSheet.columns; j < cnt2; ++j)
                {
                    var headerItem = headerItems[j];
                    if (ShouldExcludeColumn(forServer, headerItem.exclusiveType)) { continue; }

                    DataCell dataCell = dataSheet[i + 1, j+ 1];

                    if (j == 0)
                    {
                        //Key
                        writer.Write("    [");
                        WriteCell(dataCell, writer);
                        writer.Write("]={");
                    }
                    if (headerItem.valueType == CellValueType.LocalizedString)
                    {
                        writer.Write("{0}{1}=", localizedStringPrefix, headerItem.name);
                    }
                    else
                    {
                        writer.Write("{0}=", headerItem.name);
                    }
                    WriteCell(dataCell, writer);
                    if (j != cnt2 - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine("},");
            }
            
            writer.WriteLine("}");
            //<---Data sheet end

            //TODO:Localization snippets, You may use your own version.
            if(dataSheet.header.ContainsLocalizedString())
            {
                writer.Write("\nfunction InitLocalizedTexts_{0}()\n    if not GetLocalizedText then return end\n    for k,v in pairs({1}) do", sheetName, sheetName);
                for(int i = 0, cnt = headerItems.Count;i< cnt; ++i)
                {
                    var headerItem = headerItems[i];
                    if (headerItem.valueType != CellValueType.LocalizedString){ continue; }
                    string key = headerItem.name;
                    writer.Write("\n        v.{0} = GetLocalizedText(fileName, v.{1}{2} or \"\")", key, localizedStringPrefix, key);
                }
                writer.Write("\n    end\nend\n\n");
                writer.WriteLine("InitLocalizedTexts_{0}()", sheetName);
            }

            writer.WriteLine("\nreturn {0}", sheetName);
        }

        private static void WriteCellValue(DataCellValue dataCellValue, TextWriter writer)
        {
            switch(dataCellValue.valueType)
            {
                case CellValueType.Byte:
                case CellValueType.Int:
                case CellValueType.Long:
                    writer.Write(dataCellValue.numValue);
                    break;
                case CellValueType.UInt:
                case CellValueType.ULong:
                    writer.Write((uint)dataCellValue.unumValue);
                    break;
                case CellValueType.Float:
                    writer.Write(dataCellValue.floatValue);
                    break;
                case CellValueType.Double:
                    writer.Write(dataCellValue.doubleValue);
                    break;
                case CellValueType.String:
                    {
                        string strToWrite = CellValueConvert.MemoryStringToFile(dataCellValue.strValue);
                        writer.Write("\"{0}\"", strToWrite);
                    }
                    break;
                case CellValueType.LocalizedString:
                    {
                        writer.Write("\"{0}\"", dataCellValue.strValue ?? "");
                    }
                    break;
                case CellValueType.Array:
                    {
                        writer.Write('{');
                        int length = dataCellValue.arrayValue.Count;
                        for(int i = 0; i < length; ++i)
                        {
                            WriteCellValue(dataCellValue.arrayValue[i], writer);
                            if (i != length - 1)
                            {
                                writer.Write(", ");
                            }
                        }
                        writer.Write('}');
                    }
                    break;
                default:
                    throw new System.NotImplementedException(dataCellValue.valueType.ToString());
            }
        }

        private static void WriteCell(DataCell dataCell, TextWriter writer)
        {
            WriteCellValue(dataCell.value, writer);
        }
    }
}
