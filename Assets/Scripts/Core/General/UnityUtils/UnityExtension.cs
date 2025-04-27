using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        var t = go.GetComponent<T>();
        if (t == null)
        {
            return go.AddComponent<T>();
        }
        return t;
    }


	//Color
	public static string ToRGBHex(this Color c)
	{
		return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
	}

	private static byte ToByte(this float f)
	{
		f = Mathf.Clamp01(f);
		return (byte)(f * 255);
	}
}
