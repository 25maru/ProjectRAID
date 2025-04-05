using System.Collections;
using UnityEngine;

/// <summary>
/// 히트스탑(일시 정지) 처리를 담당하는 싱글톤 클래스
/// </summary>
public class HitStopManager : MonoSingleton<HitStopManager>
{
    [SerializeField] private float hitStopDuration = 0.075f;

    private bool isHitStopActive = false;

    public void DoHitStop(float duration = 0f)
    {
        duration = duration <= 0f ? hitStopDuration : duration;

        if (!isHitStopActive)
        {
            StartCoroutine(HitStopCoroutine(duration));
        }
    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        isHitStopActive = true;

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        isHitStopActive = false;
    }
}
