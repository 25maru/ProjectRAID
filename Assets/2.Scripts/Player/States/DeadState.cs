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
        player.SetMoveInput(Vector2.zero);
        player.Animator.SetTrigger(PlayerAnimatorParams.Dead);
        player.Input.DeactivateInput();
    }

    public override void Exit()
    {
        // TODO: 상태 종료 시 처리할 내용이 있다면 여기에 작성
        // 입력 시스템 재활성화, 리스폰 관련 초기화 등
    }

    public override void HandleInput()
    {
        // 입력 무시
    }

    public override void Update()
    {
        // 상태 고정, 전이 없음 - 외부(GameManager 등)에서 상태 전이 제어 예정
    }
}
