using UnityEngine;

public class GameMainState : BaseState
{

    protected override void onEnter()
    {
        Debuger.Log("GameMainState Enter");
        SceneMgr.Instance.ChangeSceneAsync(Scene.MainScene, OnSceneLoadCompleted);
    }

    public void OnSceneLoadCompleted()
    {
        
    }

    protected override void onLeave()
    {
        Debuger.Log("GameMainState Leave");
    }
}