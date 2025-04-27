using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class TestDictionary : MonoBehaviour
{
    Dictionary<int, int> test = new Dictionary<int, int>(){
        {1, 1},
        {2, 2},
        {3, 3},
        {4, 4},
        {5, 5},
        {6, 6},
        {7, 7},
    };

    void Start()
    {
        // var enumerator = test.GetEnumerator();

        // while (enumerator.MoveNext())
        // {
        //     var item = enumerator.Current;
        //     Debuger.Log("item.key: " + item.Key);
        // }
    }
}
