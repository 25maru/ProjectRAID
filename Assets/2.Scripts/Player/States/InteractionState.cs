using UnityEngine;

/// <summary>
/// 상호작용 상태: 특정 대상과 상호작용 처리 및 대기 상태로 복귀
/// </summary>
public class InteractionState : PlayerState
{
    private readonly float interactionDuration = 1f;
    private float timer;
    private bool interactionStarted;

    public InteractionState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }


    public override void Enter()
    {
        timer = 0f;
        interactionStarted = false;
        player.SetMoveInput(Vector2.zero);
        player.Animator.SetTrigger(PlayerAnimatorParams.Interact);

    }

    public override void Exit()
    {
        player.CurrentInteractable?.HideHighlight();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (!interactionStarted && timer >= 0.25f)
        {
            // 애니메이션이 어느 정도 진행된 후 상호작용 실행
            interactionStarted = true;
            player.CurrentInteractable?.Interact(player);
        }

        if (timer >= interactionDuration)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void HandleInput() { }
    public override void PhysicsUpdate() { }
}
