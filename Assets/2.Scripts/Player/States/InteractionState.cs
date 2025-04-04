using UnityEngine;

/// <summary>
/// 상호작용 상태: 특정 대상과 상호작용 후 Idle로 전환
/// </summary>
public class InteractionState : PlayerState
{
    private readonly float interactionTime = 2f;
    private float timer;

    public InteractionState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        timer = 0f;
        player.animator.SetTrigger("Interact"); // 필요시 Animator 파라미터 분리 가능
        Debug.Log("상호작용 시작");
    }

    public override void HandleInput()
    {
        // 입력 잠금
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interactionTime)
        {
            Debug.Log("상호작용 완료");
            stateMachine.ChangeState(player.idleState);
        }
    }
}
