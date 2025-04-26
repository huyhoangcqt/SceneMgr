using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIInteraction : MonoBehaviour
{
    private MainUIInteraction _instance;
    public MainUIInteraction Instance => _instance;
    private void Awake() {
        _instance = this;
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("OnPlayButtonClicked!");
        GameManager.Instance.StartBatte();
    }
}
