using UnityEngine;

/// <summary>
/// 데미지를 받을 수 있는 대상이 구현해야 할 인터페이스
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// 데미지를 받고, 공격자의 위치 기준으로 넉백 등을 처리
    /// </summary>
    /// <param name="amount">받는 데미지</param>
    /// <param name="attackerPosition">공격자의 위치</param>
    void TakeDamage(int amount, Vector3 attackerPosition);
}
