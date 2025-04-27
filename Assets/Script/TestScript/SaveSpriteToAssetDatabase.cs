using UnityEngine;
using UnityEditor;

public class SaveSpriteToAssetDatabase
{
    // public Sprite spriteToSave;
    public static string savePath = "Assets/AssetResources/Textures/";

    public static void SaveSprite(Sprite spriteToSave)
    {
        if (spriteToSave != null)
        {
            // Ensure the save path exists
            if (!AssetDatabase.IsValidFolder(savePath))
            {
                AssetDatabase.CreateFolder("Assets", "Sprites");
            }

            // Combine the save path with the sprite name
            string assetPath = savePath + spriteToSave.name + ".asset";

            // Save the sprite to the AssetDatabase
            AssetDatabase.CreateAsset(spriteToSave, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("No sprite to save.");
        }
    }
}
