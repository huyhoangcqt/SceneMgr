using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonoSingletonTemplate<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            if (FindObjectOfType<T>() != null)
            {
                instance = FindObjectOfType<T>() as T;
            }
            else
            {
                instance = new GameObject(typeof(T).FullName).AddComponent<T>();
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
