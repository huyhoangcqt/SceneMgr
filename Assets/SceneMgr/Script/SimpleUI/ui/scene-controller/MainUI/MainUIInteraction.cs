using System.Collections;
using UnityEngine;
using YellowCat.SceneMgr;

public class MainUIInteraction : MonoBehaviour
{
    private MainUIInteraction _instance;
    public MainUIInteraction Instance => _instance;
    private void Awake() {
        _instance = this;
    }

    public void OnPlayButtonClicked()
    {
        //Debug.Log("OnPlayButtonClicked!");
        ApplicationMgr.Instance.StartBatte();
    }
}
