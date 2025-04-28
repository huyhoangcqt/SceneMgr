using UnityEngine;
using System.Collections.Generic;

public class LineUp
{
    public GameObject mGo;
    public Transform mTransform => mGo.transform;
    public const int NUM = 6;
    public List<Transform> position;
    public List<Transform> objectsHideInPlayMode;
    public List<Transform> frontRow;
    public List<Transform> backRow;
    public List<Transform> leftColumn;
    public List<Transform> middleColumn;
    public List<Transform> rightColumn;

    public LineUp(GameObject gameObject)
    {
        Init(gameObject);
    }

    private void Init(GameObject root)
    {
        this.mGo = root;
        position = new List<Transform>();
        // frontRow = new List<Transform>();
        // backRow = new List<Transform>();
        // leftColumn = new List<Transform>();
        // middleColumn  new List<Transform>();
        // rightColumn = new List<Transform>();
        Transform pos0 = mTransform.Find("FrontRow/0/model_rotation");
        Transform pos1 = mTransform.Find("FrontRow/1/model_rotation (1)");
        Transform pos2 = mTransform.Find("FrontRow/2/model_rotation (2)");
        Transform pos3 = mTransform.Find("BackRow/3/model_rotation (3)");
        Transform pos4 = mTransform.Find("BackRow/4/model_rotation (4)");
        Transform pos5 = mTransform.Find("BackRow/5/model_rotation (5)");
        position.Add(pos0);
        position.Add(pos1);
        position.Add(pos2);
        position.Add(pos3);
        position.Add(pos4);
        position.Add(pos5);
        objectsHideInPlayMode = new List<Transform>(6){
            mTransform.Find("FrontRow/0/PositionShow"),
            mTransform.Find("FrontRow/1/PositionShow"),
            mTransform.Find("FrontRow/2/PositionShow"),
            mTransform.Find("BackRow/3/PositionShow"),
            mTransform.Find("BackRow/4/PositionShow"),
            mTransform.Find("BackRow/5/PositionShow")
        };
    }

    public void HideShowObjects()
    {
        for (int i = 0; i < NUM; i++){
            objectsHideInPlayMode[i].gameObject.SetActive(false);
        }
    }
}