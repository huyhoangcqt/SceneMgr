using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager instance;
    public static CoroutineManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public static Coroutine startCoroutine(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }

    public static void stopCoroutine(IEnumerator coroutine)
    {
        Instance.StopCoroutine(coroutine);
    }

    public static void stopAllCoroutines()
    {
        Instance.StopAllCoroutines();
    }

}