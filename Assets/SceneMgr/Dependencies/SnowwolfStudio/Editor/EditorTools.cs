using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorTools
{
    [MenuItem("Tools/Open Persistent Folder")]
    public static void OpenPersistentFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
