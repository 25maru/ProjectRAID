using UnityEngine;

/// <summary>
/// Animator 파라미터 해시값을 관리하는 클래스
/// </summary>
public static class PlayerAnimatorParams
{
    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int Dead = Animator.StringToHash("Dead");
    public static readonly int Hit = Animator.StringToHash("Hit");
    public static readonly int Interact = Animator.StringToHash("Interact");

    /// <summary>
    /// 콤보 공격용 애니메이션 트리거 해시값 배열
    /// Attack1 → [0], Attack2 → [1], ...
    /// </summary>
    public static readonly int[] Attacks = new int[]
    {
        Animator.StringToHash("Attack1"),
        Animator.StringToHash("Attack2"),
        Animator.StringToHash("Attack3")
    };
}