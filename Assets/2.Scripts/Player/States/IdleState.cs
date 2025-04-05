using UnityEngine;

/// <summary>
/// 대기 상태: 이동 가능, 공격/상호작용 가능
/// </summary>
public class IdleState : PlayerState
{
    private float currentSpeed;

    public IdleState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        currentSpeed = 0f;
        player.Animator.SetFloat(PlayerAnimatorParams.Speed, 0f);
    }

    public override void HandleInput()
    {
        Vector2 input = player.Input.actions["Move"].ReadValue<Vector2>();
        player.SetMoveInput(input); // 이동 입력 저장

        if (input.sqrMagnitude > 0.01f && player.CharacterController.isGrounded)
        {
            // 이동 입력이 있는 경우 이동 처리 계속 진행
        }

        if (player.Input.actions["Attack"].WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.AttackState);
            return;
        }

        if (player.Input.actions["Interact"].WasPerformedThisFrame() && player.CurrentInteractable != null)
        {
            stateMachine.ChangeState(player.InteractionState);
            return;
        }
    }

    public override void Update()
    {
        Vector2 moveInput = player.MoveInput;
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (move.sqrMagnitude > 0.01f)
        {
            float speed = player.IsRunning ? player.RunSpeed : player.MoveSpeed;
            Vector3 direction = Quaternion.Euler(0, player.transform.eulerAngles.y, 0) * move;
            player.CharacterController.Move(Time.deltaTime * speed * direction);

            // 회전 처리
            Quaternion toRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = Quaternion.RotateTowards(
                player.transform.rotation,
                toRotation,
                player.RotationSpeed * Time.deltaTime);

            // 애니메이션 Blend
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 10f);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 10f);
        }

        player.Animator.SetFloat(PlayerAnimatorParams.Speed, currentSpeed);
    }

    public override void PhysicsUpdate()
    {
        Vector3 gravity = Vector3.down * 9.8f;
        player.CharacterController.Move(gravity * Time.fixedDeltaTime);
    }
}
