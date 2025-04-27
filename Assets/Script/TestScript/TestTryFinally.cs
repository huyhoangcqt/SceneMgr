using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestTryFinally : MonoBehaviour
{
    public void TryNow()
    {
        try
        {
            Debug.Log("Try start");
            int a = 10;
            int b = 1;
            int x = a / b;
            Debug.Log("Try end");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            Debug.Log("Finally block is executed");
        }
        Debug.Log("Last line");

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        TryNow();
    }
}
