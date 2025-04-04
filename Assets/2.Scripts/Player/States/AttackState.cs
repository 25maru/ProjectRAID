using UnityEngine;

/// <summary>
/// 공격 상태: 입력 잠금, 애니메이션 재생 후 Idle 복귀
/// </summary>
public class AttackState : PlayerState
{
    private bool isAnimationFinished = false;

    public AttackState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        isAnimationFinished = false;

        player.animator.SetTrigger(PlayerAnimatorParams.Attack);

        // 공격 중 캐릭터 이동 방지
        player.moveInput = Vector2.zero;

        // 여기선 애니메이션 이벤트로 복귀 처리
    }

    public override void HandleInput()
    {
        // 공격 중에는 입력 무시
    }

    public override void Update()
    {
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    /// <summary>
    /// 애니메이션 이벤트에서 호출됨
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        isAnimationFinished = true;
    }
}
