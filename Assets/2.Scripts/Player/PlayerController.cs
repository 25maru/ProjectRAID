using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 캐릭터 제어 및 상태 관리 메인 클래스
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [FoldoutGroup("이동", ExtendedColor.White)]
    [SerializeField] private float moveSpeed = 2f;                      // 걷기 속도
    [SerializeField] private float runSpeed = 6f;                       // 달리기 속도
    [SerializeField] private float rotationSpeed = 720f;                // 회전 속도

    [FoldoutGroup("넉백", ExtendedColor.Crimson)]
    [SerializeField] private float knockbackDuration = 0.2f;            // 넉백 지속 시간
    [SerializeField] private float knockbackPower = 5f;                 // 넉백 힘
    private Vector3 knockbackDirection;                                 // 넉백 방향
    private float knockbackTimer;                                       // 넉백 타이머

    [FoldoutGroup("피격 무적 시간", ExtendedColor.Crimson)]
    [SerializeField] private float invincibleDuration = 1f;             // 피격 후 무적 시간
    private bool isInvincible = false;                                  // 현재 무적 상태 여부
    private float invincibleTimer = 0f;                                 // 무적 시간 타이머

    [FoldoutGroup("공격 판정", ExtendedColor.GreenYellow)]
    [SerializeField] private float attackRadius = 1.5f;                 // 공격 반경

    [FoldoutGroup("상호작용", ExtendedColor.GreenYellow)]
    [SerializeField] private float interactRange = 2f;                  // 상호작용 거리
    [SerializeField] private LayerMask interactableLayer;               // 상호작용 가능한 레이어
    private IInteractable currentInteractable;                          // 현재 상호작용 대상

    [FoldoutGroup("스탯", ExtendedColor.LimeGreen)]
    [SerializeField] private int maxHealth = 100;                       // 최대 체력
    private int currentHealth;                                          // 현재 체력

    [FoldoutGroup("컴포넌트", ExtendedColor.SeaGreen)]
    [SerializeField] private CharacterController characterController;   // 캐릭터 컨트롤러
    [SerializeField] private PlayerInputHandler inputHandler;           // 인풋 핸들러
    [SerializeField] private Animator animator;                         // 애니메이터

    [FoldoutGroup("상태 머신", ExtendedColor.DodgerBlue)]
    private PlayerStateMachine stateMachine;                            // 상태 머신
    private IdleState idleState;                                        // 대기 상태
    private AttackState attackState;                                    // 공격 상태
    private DeadState deadState;                                        // 사망 상태
    private InteractionState interactionState;                          // 상호작용 상태

    // 이동
    public float MoveSpeed => moveSpeed;
    public float RunSpeed => runSpeed;
    public float RotationSpeed => rotationSpeed;
    public Vector2 MoveInput => inputHandler.MoveInput;
    public bool IsRunning => inputHandler.IsRunning;

    // 컴포넌트
    public CharacterController CharacterController => characterController;
    public PlayerInputHandler InputHandler => inputHandler;
    public Animator Animator => animator;

    // 상태 머신
    public PlayerStateMachine StateMachine => stateMachine;
    public IdleState IdleState => idleState;
    public AttackState AttackState => attackState;
    public DeadState DeadState => deadState;
    public InteractionState InteractionState => interactionState;

    // 상호작용
    public IInteractable CurrentInteractable => currentInteractable;

    private void Awake()
    {
        InitializeComponents();
        InitializeStates();
    }

    /// <summary>
    /// 주요 컴포넌트를 캐싱합니다.
    /// </summary>
    private void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    /// <summary>
    /// 상태 인스턴스를 초기화합니다.
    /// </summary>
    private void InitializeStates()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new IdleState(this, stateMachine);
        attackState = new AttackState(this, stateMachine);
        deadState = new DeadState(this, stateMachine);
        interactionState = new InteractionState(this, stateMachine);
    }

    /// <summary>
    /// 초기 상태 설정
    /// </summary>
    private void Start()
    {
        currentHealth = maxHealth;
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        CheckForInteractables();
        HandleKnockback();
        HandleInvincibility();
        UpdateStateMachine();
    }

    /// <summary>
    /// 넉백 처리
    /// </summary>
    private void HandleKnockback()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            characterController.Move(knockbackDirection * knockbackPower * Time.deltaTime);
        }
    }

    /// <summary>
    /// 피격 무적 처리
    /// </summary>
    private void HandleInvincibility()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    /// <summary>
    /// 상태 머신 입력 및 업데이트 호출
    /// </summary>
    private void UpdateStateMachine()
    {
        if (knockbackTimer <= 0)
        {
            stateMachine.CurrentState.HandleInput();
            stateMachine.CurrentState.Update();
        }
    }
    
    /// <summary>
    /// 물리 업데이트 (필요 시 상태에서 구현)
    /// </summary>
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
        // 죽었거나 무적이면 데미지 무시
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
            HitStopManager.Instance.DoHitStop();
            CameraShakeManager.Instance.Shake();
        }
    }

    public void SetMoveInput(Vector2 input)
    {
        // 유지 - 외부에서 MoveInput을 오버라이드할 경우 대비
    }

    /// <summary>
    /// 주변 상호작용 대상 탐지 및 하이라이트 처리
    /// </summary>
    private void CheckForInteractables()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);

        IInteractable nearest = null;
        float minDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<IInteractable>(out var interactable))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = interactable;
                }
            }
        }

        if (nearest != currentInteractable)
        {
            currentInteractable?.HideHighlight();
            nearest?.ShowHighlight();
            currentInteractable = nearest;
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
