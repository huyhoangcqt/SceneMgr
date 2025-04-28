using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIUtils
{
    public static GameObject[] GetChildren(GameObject parent, GameObject prefab, int count)
    {
        GameObject[] children = new GameObject[count];
        Transform parentTrans = parent.transform;

        int getIndex = 0;
        for (int i = 0, cnt = parentTrans.childCount; i < cnt; ++i)
        {
            Transform child = parentTrans.GetChild(i);
            if (!child)
            {
                continue;
            }
            GameObject childGO = child.gameObject;
            if (childGO == prefab) { continue; }

            childGO.SetActive(getIndex < count);
            if (getIndex < count)
            {
                children[getIndex++] = childGO;
            }
        }

        for (int i = getIndex; i < count; ++i)
        {
            GameObject newItem = GameObject.Instantiate<GameObject>(prefab, parentTrans, false);
            newItem.SetActive(true);
            children[getIndex++] = newItem;
        }

        prefab.gameObject.SetActive(false);

        return children;
    }
}
