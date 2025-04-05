using UnityEngine;

/// <summary>
/// 공격 상태: 입력 잠금, 애니메이션 재생 후 Idle 복귀
/// </summary>
public class AttackState : PlayerState
{
    private bool canCombo = false;
    private bool isAnimationFinished = false;
    private bool receivedNextComboInput = false;

    private int currentCombo = 0;
    private readonly int maxCombo = 3;

    private readonly float attackRadius = 1.5f;

    public AttackState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        receivedNextComboInput = false;
        isAnimationFinished = false;
        canCombo = false;

        currentCombo = 1;
        PlayComboAnimation(currentCombo);

        // 공격 중 캐릭터 이동 방지
        player.SetMoveInput(Vector2.zero);
    }

    public override void Exit()
    {
        canCombo = false;
        receivedNextComboInput = false;
        isAnimationFinished = false;
    }

    public override void HandleInput()
    {
        if (canCombo && player.Input.actions["Attack"].WasPressedThisFrame())
        {
            receivedNextComboInput = true;
        }
    }

    public override void Update()
    {
        if (isAnimationFinished)
        {
            if (receivedNextComboInput && currentCombo < maxCombo)
            {
                currentCombo++;
                receivedNextComboInput = false;
                isAnimationFinished = false;
                canCombo = false;

                PlayComboAnimation(currentCombo);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    /// <summary>
    /// 현재 콤보 인덱스에 맞는 공격 애니메이션 재생
    /// </summary>
    public void PlayComboAnimation(int comboIndex)
    {
        if (comboIndex <= PlayerAnimatorParams.Attacks.Length)
        {
            int hash = PlayerAnimatorParams.Attacks[comboIndex - 1];
            player.Animator.SetTrigger(hash);
        }
        else
        {
            Debug.LogWarning("콤보 인덱스가 최대치를 초과했습니다.");
        }
    }

    /// <summary>
    /// 애니메이션 이벤트: 실제 공격 판정 처리
    /// </summary>
    public void PerformAttack()
    {
        Vector3 origin = player.transform.position + player.transform.forward;
        Collider[] hits = Physics.OverlapSphere(origin, attackRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(20, player.transform.position);
                HitStopManager.Instance.DoHitStop();
                CameraShakeManager.Instance.Shake();
            }
        }
    }

    /// <summary>
    /// 애니메이션 이벤트: 콤보 입력 받을 수 있는 구간 진입
    /// </summary>
    public void EnableComboWindow()
    {
        canCombo = true;
    }

    /// <summary>
    /// 애니메이션 이벤트: 현재 공격 애니메이션 종료
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        isAnimationFinished = true;
    }
}
