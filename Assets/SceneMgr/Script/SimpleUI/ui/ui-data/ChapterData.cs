using System.Collections.Generic;

public class ChapterData
{
    public const int STAGE_COUNT = 20;
    public List<StageItemData> stages;

    public ChapterData(string key)
    {
        stages = new List<StageItemData>();
        for (int i = 1; i <= STAGE_COUNT; i++)
        {
            stages.Add(new StageItemData(i));
        }
    }
}