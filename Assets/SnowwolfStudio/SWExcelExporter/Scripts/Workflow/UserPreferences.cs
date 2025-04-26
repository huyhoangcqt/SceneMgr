using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Snowwolf
{
    public class UserPreferences
    {
        public string excelPath;
        public string clientPath;
        public string serverPath;
        public List<string> lastSelectedExcelFiles = new List<string>();
        public List<string> clientUsingExporters = new List<string>();
        public List<string> serverUsingExporters = new List<string>();

        private static string prefFilePath 
        {
            get
            {
                //Using hash of Application.dataPath so that you can use multi instance of this App from different place.
                string fileName = string.Format("userpref_{0}.json", Hash128.Compute(Application.dataPath).ToString());
                return Path.Combine(Application.persistentDataPath, fileName);
            }
        } 
        private static UserPreferences s_Instance;

        private static UserPreferences CreateNew()
        {
            UserPreferences newPref = new UserPreferences();
            //TODO: You can set the initial values according to your project structure.
            #if UNITY_STANDALONE_OSX
            newPref.excelPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../../.."));
            newPref.clientPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../../../Output/Client"));
            // newPref.serverPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../../../Output/Server"));
            #else
            newPref.excelPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../.."));
            newPref.clientPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../../Output/Client"));
            // newPref.serverPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../../Output/Server"));
            #endif
            return newPref;
        }

        public static UserPreferences GetOrLoad()
        {
            if (s_Instance != null) { return s_Instance; }
            string fullPath = prefFilePath;
            try
            {
                if (File.Exists(fullPath))
                {
                    string jsonText = File.ReadAllText(fullPath, new UTF8Encoding(false));
                    s_Instance = JsonUtility.FromJson<UserPreferences>(jsonText);
                }
            }
            catch(System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
            if (s_Instance == null)
            {
                s_Instance = CreateNew();
            }

            return s_Instance;
        }

        public static void Save()
        {
            if (s_Instance == null)
            {
                s_Instance = CreateNew();
            }
            
            try
            {
                File.WriteAllText(prefFilePath, JsonUtility.ToJson(s_Instance), new UTF8Encoding(false));
            }
            catch(System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}
