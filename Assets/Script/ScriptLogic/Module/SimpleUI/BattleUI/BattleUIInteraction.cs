using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIInteraction : MonoBehaviour
{
    private BattleUIInteraction _instance;
    public BattleUIInteraction Instance => _instance;
    private void Awake() {
        _instance = this;
    }

    public void OnHomeButtonClicked()
    {
        Debug.Log("OnHomeButtonClicked!");
        GameManager.Instance.GoHome();
    }
}
