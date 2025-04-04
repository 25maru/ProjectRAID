/// <summary>
/// 플레이어 FSM 상태 전이 처리 클래스
/// </summary>
public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void HandleInput() => CurrentState?.HandleInput();
    public void Update() => CurrentState?.Update();
}
