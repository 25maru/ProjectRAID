using UnityEngine;

/// <summary>
/// 사망 상태: 입력 불가, 사망 애니메이션 후 자동 복귀
/// </summary>
public class DeadState : PlayerState
{
    private float deadTime = 0f;
    private readonly float respawnDelay = 5f;

    public DeadState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        player.animator.SetTrigger(PlayerAnimatorParams.Dead);
        deadTime = 0f;
    }

    public override void HandleInput()
    {
        // 입력 무시
    }

    public override void Update()
    {
        deadTime += Time.deltaTime;
        if (deadTime >= respawnDelay)
        {
            // 임시: 그냥 Idle로 돌아가기
            stateMachine.ChangeState(player.idleState);
        }
    }
}
