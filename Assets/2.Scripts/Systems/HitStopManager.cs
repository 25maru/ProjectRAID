using System.Collections;
using UnityEngine;

/// <summary>
/// 히트스탑(일시 정지) 처리를 담당하는 싱글톤 클래스
/// </summary>
public class HitStopManager : MonoSingleton<HitStopManager>
{
    private bool isHitStopActive = false;

    public void DoHitStop(float duration = 0.05f)
    {
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
