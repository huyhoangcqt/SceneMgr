using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Main,
    Battle,
}

public class GameManager : Singleton<GameManager>
{
    private StateMachine mMachine;

    public GameManager() : base()
    {
        Init();
    }

    private void Init()
    {
        mMachine = new StateMachine();
        mMachine.RegisterState(GameState.Main, new GameMainState());
        mMachine.RegisterState(GameState.Battle, new GameBattleState());

    }

    public void Start()
    {
        mMachine.ChangeState(GameState.Main);
    }

    public void StartBatte()
    {
        mMachine.ChangeState(GameState.Battle);
    }

    public void GoHome()
    {
        mMachine.ChangeState(GameState.Main);
    }
}