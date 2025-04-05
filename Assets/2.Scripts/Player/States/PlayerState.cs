using UnityEngine;

/// <summary>
/// 플레이어 상태 기본 클래스 (모든 상태의 기반)
/// </summary>
public abstract class PlayerState
{
    protected readonly PlayerController player;
    protected readonly PlayerStateMachine stateMachine;

    public PlayerState(PlayerController player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// 상태에 진입할 때 호출됩니다.
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// 상태에서 나올 때 호출됩니다.
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// 입력을 처리합니다.
    /// </summary>
    public virtual void HandleInput() { }

    /// <summary>
    /// 매 프레임마다 호출되는 일반 업데이트입니다.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// FixedUpdate에서 호출되는 물리 업데이트입니다.
    /// </summary>
    public virtual void PhysicsUpdate() { }
}