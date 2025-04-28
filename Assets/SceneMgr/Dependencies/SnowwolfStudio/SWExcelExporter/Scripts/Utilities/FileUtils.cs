// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Snowwolf
{
    public static class FileUtils
    {
        public static string workTempPath
        {
            get
            {
                string tempPath = Path.Combine(Application.persistentDataPath, "Temp");
                return ResolveDirectory(tempPath);
            }
        }

        public static string exportOutTempFolder
        {
            get
            {
                string tempPath = Path.Combine(Application.persistentDataPath, "Temp/ExportOut");
                return ResolveDirectory(tempPath);
            }
        }
        
        public static void CleanFolder(string targetFolder, bool includeSelf = false)
        {
            if (Directory.Exists(targetFolder))
            {
                Directory.Delete(targetFolder, true);
                if (!includeSelf)
                {
                    ResolveDirectory(targetFolder);
                }
            }
        }

        public static string[] GetAllExcelFiles(string targetPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (string.IsNullOrEmpty(targetPath) || !Directory.Exists(targetPath))
            {
                return new string[0];
            }

            List<string> files = new List<string>();
            foreach (string file in Directory.GetFiles(targetPath, "*.xlsx", searchOption))
            {
                string rawFileName = Path.GetFileName(file);
                if (rawFileName.StartsWith("~$")) { continue; }
                files.Add(file);
            }

            return files.ToArray();
        }

        public static bool CopyTree(string srcDir, string searchPattern, string dstDir)
        {
            if (string.IsNullOrEmpty(srcDir) || !Directory.Exists(srcDir) || string.IsNullOrEmpty(dstDir))
            {
                return false;
            }
            
            srcDir = Path.GetFullPath(srcDir);
            foreach (var filePath in Directory.GetFiles(srcDir, searchPattern, SearchOption.AllDirectories))
            {
                string relativePath = filePath.Substring(srcDir.Length + 1);
                string targetFilePath = Path.Combine(dstDir, relativePath);
                string targetFolder = Path.GetDirectoryName(targetFilePath);
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }
                File.Copy(filePath, targetFilePath, true);
            }

            return true;
        }

        public static string ResolveFileDirectory(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        public static string ResolveDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }    
}
