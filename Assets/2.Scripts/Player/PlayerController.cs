using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 캐릭터 제어 및 상태 관리 메인 클래스
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("이동 속도")]
    public float moveSpeed = 5f;

    [Header("컴포넌트")]
    public Animator animator;
    public CharacterController characterController;
    public PlayerInput input;

    [HideInInspector] public Vector2 moveInput;

    public PlayerStateMachine stateMachine;

    // 상태 인스턴스
    public IdleState idleState;
    public AttackState attackState;
    public DeadState deadState;
    public InteractionState interactionState;
    public int maxHealth = 100;
    public int currentHealth;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
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
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            TakeDamage(50);
        }

        stateMachine.HandleInput();
        stateMachine.Update();
    }

    public void PerformAttack()
    {
        if (stateMachine.CurrentState is AttackState attackState)
        {
            attackState.PerformAttack();
        }
    }

    /// <summary>
    /// 공격 애니메이션 종료 시 Animator Event에서 호출됨
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        if (stateMachine.CurrentState is AttackState attackState)
        {
            attackState.OnAttackAnimationEnd();
        }
    }

    /// <summary>
    /// 피격 테스트용 메서드 (임시)
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (stateMachine.CurrentState == deadState) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            stateMachine.ChangeState(deadState);
        }
        else
        {
            animator.SetTrigger("Hit"); // 피격 애니메이션 트리거
        }
    }
}
