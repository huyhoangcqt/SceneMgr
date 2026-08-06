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
        stageViewCtrl = new DynamicListView<StageItem>(stageView);
        stageViewCtrl.InitView(cData.stages);
    }
}
