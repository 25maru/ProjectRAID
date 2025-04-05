using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 입력을 캡슐화하여 이벤트 기반으로 처리하는 헬퍼 클래스
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    /// <summary>
    /// 달리기 입력 상태 (외부에서 접근 가능)
    /// </summary>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// 현재 프레임 이동 입력값
    /// </summary>
    public Vector2 MoveInput => playerInput.actions["Move"].ReadValue<Vector2>();

    /// <summary>
    /// 이번 프레임에 공격 입력이 있었는지 여부
    /// </summary>
    public bool IsAttackPressed => playerInput.actions["Attack"].WasPerformedThisFrame();

    /// <summary>
    /// 이번 프레임에 상호작용 입력이 있었는지 여부
    /// </summary>
    public bool IsInteractPressed => playerInput.actions["Interact"].WasPerformedThisFrame();

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["Run"].started += OnRunStarted;
        playerInput.actions["Run"].canceled += OnRunCanceled;
    }

    private void OnDisable()
    {
        playerInput.actions["Run"].started -= OnRunStarted;
        playerInput.actions["Run"].canceled -= OnRunCanceled;
    }

    private void OnRunStarted(InputAction.CallbackContext context)
    {
        IsRunning = true;
    }

    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        IsRunning = false;
    }
}