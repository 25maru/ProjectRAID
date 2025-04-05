using UnityEngine;

/// <summary>
/// Idle 상태: 이동 가능, 공격/상호작용 가능
/// </summary>
public class IdleState : PlayerState
{
    public IdleState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void HandleInput()
    {
        player.moveInput = player.input.actions["Move"].ReadValue<Vector2>();

        if (player.input.actions["Attack"].WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.attackState);
            return;
        }

        if (player.input.actions["Interact"].WasPressedThisFrame())
        {
            if (IsNearInteractable())
            {
                stateMachine.ChangeState(player.interactionState);
            }
        }
    }

    private bool IsNearInteractable()
    {
        Ray ray = new(player.transform.position + Vector3.up, player.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            return hit.collider.CompareTag("Interactable");
        }
        return false;
    }

    public override void Update()
    {
        Vector3 move = new(player.moveInput.x, 0, player.moveInput.y);
        if (move.magnitude > 0.1f)
        {
            player.characterController.Move(Time.deltaTime * player.moveSpeed * move.normalized);
            player.transform.forward = move.normalized;

            player.animator.SetFloat("Speed", 1f);
        }
        else
        {
            player.animator.SetFloat("Speed", 0f);
        }
    }
}
