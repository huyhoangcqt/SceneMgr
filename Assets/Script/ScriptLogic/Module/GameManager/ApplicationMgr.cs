using Unity.VisualScripting;
using UnityEngine;
using YellowCat.SceneMgr;

public enum GameState
{
    Main,
    Battle,
}

public class ApplicationMgr : Singleton<ApplicationMgr>
{
    public ApplicationMgr() : base ()
    {
    }


    public void Start()
    {
		SceneMgr.Instance.LoadScene(Scene.MainScene);
	}

    public void StartBatte()
	{
		SceneMgr.Instance.LoadScene(Scene.BattleScene);
	}

    public void GoHome()
	{
		SceneMgr.Instance.LoadScene(Scene.MainScene);
	}
}