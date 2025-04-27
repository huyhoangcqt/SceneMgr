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
		SceneMgr.Instance.LoadScene("MainScene");
	}

    public void StartBatte()
	{
		SceneMgr.Instance.LoadScene("BattleScene");
	}

    public void GoHome()
	{
		SceneMgr.Instance.LoadScene("MainScene");
	}
}