using UnityEngine;
using System.Collections.Generic;
using ExcelData;

public class ChapterData
{
    Chapter.Item config;
    public List<Stage.Item> stages;

    public ChapterData(string key)
    {
        config = Chapter.GetItem(key);
        stages = new List<Stage.Item>();
        foreach (var entry in Stage.GetDict()){
            var stageCfg = entry.Value;
            if (stageCfg.chapter == key){
                stages.Add(stageCfg);
            }
        }
    }
}