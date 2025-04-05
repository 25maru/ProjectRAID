using UnityEngine;

/// <summary>
/// 사망 상태: 입력 불가, 사망 애니메이션 재생
/// </summary>
public class DeadState : PlayerState
{
    public DeadState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player.Animator.SetTrigger(PlayerAnimatorParams.Dead);

        // TODO: 상태 진입 시 처리할 내용이 있다면 여기에 작성
        // UI 호출, 리스폰 대기 등
    }

    public override void Exit()
    {
        // TODO: 상태 종료 시 처리할 내용이 있다면 여기에 작성
        // 인풋 시스템 재활성화, 리스폰 관련 초기화 등
    }

    public override void HandleInput() { }
    public override void Update() { }
    public override void PhysicsUpdate() { }
}
