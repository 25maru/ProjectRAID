using UnityEngine;

/// <summary>
/// 상호작용 상태: 특정 대상과 상호작용 후 Idle로 전환
/// </summary>
public class InteractionState : PlayerState
{
    private bool isInteractionFinished = false;

    public InteractionState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        isInteractionFinished = false;
        player.SetMoveInput(Vector2.zero);
        player.Animator.SetTrigger(PlayerAnimatorParams.Interact);
        player.CurrentInteractable?.Interact(player);
    }

    public override void HandleInput()
    {
        // 입력 무시
    }

    public override void Update()
    {
        if (isInteractionFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    /// <summary>
    /// 애니메이션 이벤트: 현재 상호작용 애니메이션 종료
    /// </summary>
    public void OnInteractionAnimationEnd()
    {
        isInteractionFinished = true;
    }
}
