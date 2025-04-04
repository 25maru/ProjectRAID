using UnityEngine;

/// <summary>
/// Animator 파라미터 해시값을 관리하는 클래스
/// </summary>
public static class PlayerAnimatorParams
{
    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Dead = Animator.StringToHash("Dead");
}