using UnityEngine;

/// <summary>
/// 공격 상태: 입력 잠금, 애니메이션 재생 후 Idle 복귀
/// </summary>
public class AttackState : PlayerState
{
    private bool isAnimationFinished = false;
    private float attackRange = 1.5f;
    private float attackRadius = 1.0f;

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

#if UNITY_EDITOR
        // 디버그용 히트박스 시각화
        Debug.DrawRay(player.transform.position + Vector3.up, player.transform.forward * attackRange, Color.red);
#endif
    }

    /// <summary>
    /// 공격 애니메이션에서 호출됨 - 타격 판정 수행
    /// </summary>
    public void PerformAttack()
    {
        Vector3 origin = player.transform.position + player.transform.forward;
        Collider[] hitColliders = Physics.OverlapSphere(origin, attackRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(20); // 공격 데미지
            }
        }
    }

    /// <summary>
    /// 공격 애니메이션 종료 시 호출됨
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        isAnimationFinished = true;
    }
}
