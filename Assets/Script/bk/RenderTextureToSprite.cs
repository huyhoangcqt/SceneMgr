using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RenderTextureToSprite : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Custom Tools/Convert Render Texture to Sprite")]
    public static void ConvertRenderTextureToSprite()
    {
        RenderTexture renderTexture = Selection.activeObject as RenderTexture;
        if (renderTexture != null)
        {
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            sprite.name = "RenderTextureSprite";
            SaveSpriteToAssetDatabase.SaveSprite(sprite);
            // GameObject spriteObject = new GameObject("RenderTextureSprite");
            // spriteObject.AddComponent<SpriteRenderer>().sprite = sprite;
        }
    }
#endif
}
