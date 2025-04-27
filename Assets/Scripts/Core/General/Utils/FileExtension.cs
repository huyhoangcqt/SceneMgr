using System.IO;
using UnityEngine;

public class FileIO
{
	//Resource File
	public static string ReadResourceFile(string fileName)
	{
		string filePath = Application.dataPath + "/" + fileName;
		return ReadFile(filePath);
	}

	public static void WriteResourceFile(string fileName, string jsonString, bool isForce = true)
	{
		string filePath = Application.dataPath + "/" + fileName;
		WriteFile(filePath, jsonString, isForce);
	}

	//Data file
	public static string ReadDataFile(string fileName)
	{
		string filePath = Application.persistentDataPath + "/" + fileName;
		return ReadFile(filePath);
	}

	public static void WriteDataFile(string fileName, string jsonString, bool isForce = true)
	{
		string filePath = Application.persistentDataPath + "/" + fileName;
		WriteFile(filePath, jsonString, isForce);
	}


	//JSON
	private static string ReadFile(string filepath)
	{
		// Does the file exist?
		if (File.Exists(filepath))
		{
			// Read the entire file and save its contents.
			string fileContents = File.ReadAllText(filepath);
			return fileContents;
		}
		return null;
	}

	public static void WriteFile(string filepath, string jsonString, bool isForce = true)
	{
		if (isForce)
		{
			string[] path = filepath.Split("/");
			string filename = path[path.Length - 1];
			string folderPath = filepath.Substring(0, filepath.Length - filename.Length);
			Debug.Log("folderPath: " +  folderPath);
			Debug.Log("fullPath: " + filepath);
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
		}

		// Write JSON to file.
		File.WriteAllText(filepath, jsonString);
	}
	//JSON!

	//BINARY
	//BINARY!
}