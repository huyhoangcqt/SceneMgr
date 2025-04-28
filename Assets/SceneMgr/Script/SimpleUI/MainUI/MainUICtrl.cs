using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MainUICtrl : MonoBehaviour
{
    private static MainUICtrl _instance;
    public static MainUICtrl Instance => _instance;
    public DynamicListViewMono stageView;
    DynamicListView<StageItem> stageViewCtrl;

    void Awake()
    {
        _instance = this;
        InitData();
    }

    private void Start() {
        InitChapterView(cData);
    }

    ChapterData cData;
    private void InitData()
    {
        cData = new ChapterData("c01");
    }

    public void InitChapterView(ChapterData cData)
    {
        List<ItemListData> stageDatas = new List<ItemListData>();
        foreach (var stageData in cData.stages){
            stageDatas.Add(new StageItemData(stageData));
        }
        stageViewCtrl = new DynamicListView<StageItem>(stageView);
        stageViewCtrl.InitView(stageDatas);
    }
}
