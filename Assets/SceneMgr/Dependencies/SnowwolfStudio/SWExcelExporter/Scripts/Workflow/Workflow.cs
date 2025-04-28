using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OfficeOpenXml;

namespace Snowwolf
{
    public static class Workflow
    {
        public static IEnumerator ExportFiles(string[] filesToExport, string targetPath, string[] usingExporters, bool forServer)
        {
            if ((filesToExport == null) || (filesToExport.Length == 0) || string.IsNullOrEmpty(targetPath) || (usingExporters == null) || (usingExporters.Length == 0))
            {
                yield break;
            }
            string exportOutTempFolder = FileUtils.exportOutTempFolder;
            FileUtils.CleanFolder(exportOutTempFolder);
            yield return null;

            for (int i = 0, length = usingExporters.Length; i < length; i++)
            {
                var exporter = Exporters.GetExporter(usingExporters[i]);
                Debug.LogFormat("--->Exporting files to '{0}' by exporter '{1}'.", exportOutTempFolder, exporter);
                for (int j = 0, fileCount = filesToExport.Length; j < fileCount; j++)
                {
                    string excelFilePath = filesToExport[j];
                    Debug.LogFormat("---->Reading excel: {0}", Path.GetFileName(excelFilePath));
                    using(FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (ExcelPackage package = new ExcelPackage(fileStream))
                        {
                            ExcelWorkbook workbook = package.Workbook;
                            foreach (var worksheet in workbook.Worksheets)
                            {
                                Debug.LogFormat("----->Transforming worksheet: {0}", worksheet.Name);
                                DataSheet dataSheet = ExcelReader.TransformDataSheet(worksheet);
                                if (dataSheet != null)
                                {
                                    // Debug.LogFormat("----->Exporting worksheet: {0}", dataSheet.sheetName);
                                    exporter.Export(exportOutTempFolder, dataSheet, forServer);
                                    // Debug.LogFormat("----->Finished exporting worksheet: {0}", dataSheet.sheetName);
                                }
                                else
                                {
                                    Debug.LogFormat("----->Transform worksheet '{0}' failed. There is nothing to export.", worksheet.Name);
                                }
                            }
                        }
                    }
                    Debug.LogFormat("---->Finish exporting excel: {0}.", Path.GetFileName(excelFilePath));
                    yield return null;
                }
            }

            Debug.LogFormat("--->Copy files from '{0}' to '{1}'.", exportOutTempFolder, targetPath);
            FileUtils.CopyTree(exportOutTempFolder, "*", targetPath);
        }
    }
}