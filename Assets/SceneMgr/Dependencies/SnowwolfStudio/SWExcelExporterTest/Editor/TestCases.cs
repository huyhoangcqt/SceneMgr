using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using OfficeOpenXml;
using Snowwolf;
using System.Reflection;

public class TestCases
{
    private static string formatDemoExcelPath => Path.Combine(Application.dataPath, "SnowwolfStudio/SWExcelExporterTest/ExcelFiles/FormatDemo.xlsx");
    private static string exporterOutputPath => Path.Combine(Application.dataPath, "SnowwolfStudio/SWExcelExporterTest/ExporterOutput");

    [MenuItem("TestCases/Test All Exporters")]
    private static void TestAllExporters()
    {
        Debug.LogFormat("->Test all exporters with file : {0}", formatDemoExcelPath);
        using(FileStream fileStream = new FileStream(formatDemoExcelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (ExcelPackage package = new ExcelPackage(fileStream))
            {
                ExcelWorkbook workbook = package.Workbook;
                foreach (var worksheet in workbook.Worksheets)
                {
                    Debug.LogFormat("-->Transforming worksheet: {0}", worksheet.Name);
                    DataSheet dataSheet = ExcelReader.TransformDataSheet(worksheet);
                    var exporters = Snowwolf.Exporters.GetSupportedExporters();
                    for (int i = 0, cnt = exporters.Length; i < cnt; ++i)
                    {
                        var exporter = exporters[i];
                        var outputPath = Path.Combine(exporterOutputPath, exporter.name);
                        Debug.LogFormat("--->Testing exporter '{0}' to '{1}'", exporter.name, outputPath);
                        exporter.Export(outputPath, dataSheet, false);
                        Debug.LogFormat("--->Testing exporter '{0}' done.", exporter.name);
                    }
                }
            }
        }
        Debug.Log("->Test all exporters done.");
    }

    [MenuItem("TestCases/Test C#+binary reading")]
    private static void TestCharpOutput()
    {
        string typeName = "ExcelData.FormatDemo";
        Assembly assembly = Assembly.Load("Assembly-CSharp");
        var formatDemoType = assembly.GetType(typeName);
        if (formatDemoType == null)
        {
            string errorMessage = string.Format("Class '{0}' do not exits, please use menu 'TestCases/Test All Exporters' to generate the associated files and then use 'CTRL/CMD+R' to refresh editor.", typeName);
            EditorUtility.DisplayDialog("Tips", errorMessage, "OK");
            return;
        }

        //NOTE:These pieces of code use reflection to avoid compiling issues. They are as same as codes below.
        // var dataDict = ExcelData.FormatDemo.GetDict();
        // foreach (var eachItem in dataDict)
        // {
        //     Debug.Log(LitJson.JsonMapper.ToJson(eachItem));
        // }
        var getDictMethod = formatDemoType.GetMethod("GetDict", BindingFlags.Public|BindingFlags.Static);
        var dataDict = (IEnumerable)getDictMethod.Invoke(null, null);
        foreach (var eachItem in dataDict)
        {
            Debug.Log(LitJson.JsonMapper.ToJson(eachItem));
        }
    }
}
