using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 캐릭터 제어 및 상태 관리 메인 클래스
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("이동")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 720f;

    [Header("넉백")]
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float knockbackPower = 5f;

    [Header("상태값")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("컴포넌트")]
    public Animator animator;
    public CharacterController characterController;
    public PlayerInput input;

    [Header("상태 머신")]
    public PlayerStateMachine stateMachine;
    public IdleState idleState;
    public AttackState attackState;
    public DeadState deadState;
    public InteractionState interactionState;

    [Header("공격 판정")]
    [SerializeField] private float attackRadius = 1.5f;
    
    [Header("피격 무적 시간")]
    [SerializeField] private float invincibleDuration = 1f;

    [HideInInspector] public Vector2 moveInput;

    // 넉백 처리
    private Vector3 knockbackDirection;
    private float knockbackTimer;

    // 무적 처리
    private bool isInvincible = false;
    private float invincibleTimer = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();

        stateMachine = new PlayerStateMachine();
        idleState = new IdleState(this, stateMachine);
        attackState = new AttackState(this, stateMachine);
        deadState = new DeadState(this, stateMachine);
        interactionState = new InteractionState(this, stateMachine);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            characterController.Move(Time.deltaTime * knockbackPower * knockbackDirection);
            return;
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }

        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    /// <summary>
    /// 애니메이션 이벤트: 실제 공격 판정 처리
    /// </summary>
    public void PerformAttack()
    {
        if (stateMachine.CurrentState is AttackState attackState)
        {
            attackState.PerformAttack();
        }
    }

    /// <summary>
    /// 애니메이션 이벤트: 콤보 입력 받을 수 있는 구간 진입
    /// </summary>
    public void EnableComboWindow()
    {
        if (stateMachine.CurrentState is AttackState attackState)
        {
            attackState.EnableComboWindow();
        }
    }

    /// <summary>
    /// 애니메이션 이벤트: 현재 공격 애니메이션 종료
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        if (stateMachine.CurrentState is AttackState attackState)
        {
            attackState.OnAttackAnimationEnd();
        }
    }

    /// <summary>
    /// IDamageable 인터페이스 구현
    /// </summary>
    public void TakeDamage(int amount, Vector3 attackerPosition)
    {
        // 죽었거나 무적 중이면 데미지 무시
        if (stateMachine.CurrentState == deadState || isInvincible) return;

        currentHealth -= amount;

        // 넉백 방향
        Vector3 dir = (transform.position - attackerPosition).normalized;
        knockbackDirection = new Vector3(dir.x, 0, dir.z);
        knockbackTimer = knockbackDuration;

        // 무적 타이머 시작
        isInvincible = true;
        invincibleTimer = invincibleDuration;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            stateMachine.ChangeState(deadState);
        }
        else
        {
            animator.SetTrigger(PlayerAnimatorParams.Hit);
            HitStopManager.Instance.DoHitStop(0.075f);
            CameraShakeManager.Instance.Shake();
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// 씬 뷰에서 공격 판정 범위 시각화
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (stateMachine != null && stateMachine.CurrentState is AttackState)
        {
            Gizmos.color = Color.red;
            Vector3 origin = transform.position + transform.forward;
            Gizmos.DrawWireSphere(origin, attackRadius);
        }
    }
#endif
}
