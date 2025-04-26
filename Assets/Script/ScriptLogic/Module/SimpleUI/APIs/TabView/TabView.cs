using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TabButton
{
    int mIndex;
    Button mButton;
    GameObject mContent;

    public TabButton(Button button, GameObject contentView, int index, Action<int> callback){
        mButton = button;
        mIndex = index;
        mContent = contentView; 

        mButton.onClick.AddListener(() => {
            if (callback != null){
                callback?.Invoke(mIndex);
            }
        });
    }

    public void SetActive(bool active){
        if (active){
            mButton.interactable = false;
            mContent.SetActive(true);
        }
        else {
            mButton.interactable = true;
            mContent.SetActive(false);
        }
    }
}

public class TabView : MonoBehaviour
{
    public List<GameObject> tabContents;
    public List<Button> tabNavButtons;
    public List<TabButton> tabButtons;

    private int activeTab;

    private void Awake()
    {
        activeTab = 0;
        tabContents[activeTab].SetActive(true);
        tabNavButtons[activeTab].interactable = false;

        tabButtons = new List<TabButton>();
        for (int i = 0; i < tabContents.Count; i++){
            TabButton tabButton = new TabButton(tabNavButtons[i], tabContents[i], i, ChangeTab);
            tabButtons.Add(tabButton);
        }
    }

    public void ChangeTab(int index)
    {
        Debuger.Log("ChangeTab: " + index);
        tabButtons[activeTab].SetActive(false);
        tabButtons[index].SetActive(true);
        activeTab = index;
    }
}
