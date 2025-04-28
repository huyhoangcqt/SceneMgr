using UnityEngine;
using UnityEngine.PlayerLoop;

public class LocalDataMgr : Singleton<LocalDataMgr>
{
    public UserLineup userLineup;

    public LocalDataMgr() : base()
    {
        Initialization();
    }

    private void Initialization()
    {
        userLineup = new UserLineup();
        userLineup.Initialization();
    }
}