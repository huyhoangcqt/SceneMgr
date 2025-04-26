using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using UnityEngine;
using UnityEngine.UI;


public class StageItem : ListViewItem
{
    StageItemData stageData;
    private UnityEngine.UI.Text mButtonLabel;
    private Transform active_outline;

    protected override void OnInitView()
    {
        active_outline = mObjView.transform.Find("active_outline");
        active_outline.gameObject.SetActive(false);

        stageData = mData as StageItemData;
        mButtonLabel = mObjView.transform.Find("text").GetComponent<UnityEngine.UI.Text>();
        mButtonLabel.text = stageData.Name;
        mObjView.name = stageData.Name;

    }

    protected override void OnItemClicked()
    {
        Debuger.Log("StateItem: " + stageData.Name + " clicked!");
        active_outline.gameObject.SetActive(true);
        UICacheData.selectedStage = stageData.Key;
    }

    public override void OnUnselected()
    {
        active_outline.gameObject.SetActive(false);
    }
}


