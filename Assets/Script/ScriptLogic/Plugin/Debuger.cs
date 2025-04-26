using UnityEngine;

public class Debuger
{
    public static void Log(string message)
    {

        Debug.Log(message);
    }

    public static void Wrn(string message)
    {

        Debug.LogWarning(message);
    }

    public static void Err(string message)
    {
        Debug.LogError(message);
    }
}