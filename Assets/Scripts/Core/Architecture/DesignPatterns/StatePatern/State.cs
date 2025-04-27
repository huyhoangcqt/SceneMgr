

public class BaseState
{
	public void Enter()
	{
		OnEnter();
	}

	public void Exit()
	{
		OnExit();
	}

	protected virtual void OnEnter()
	{

	}

	protected virtual void OnExit()
	{

	}
}
