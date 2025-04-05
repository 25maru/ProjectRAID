using UnityEngine;
using Cinemachine;

/// <summary>
/// 카메라 셰이크를 관리하는 매니저
/// </summary>
public class CameraShakeManager : MonoSingleton<CameraShakeManager>
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeAmplitude = 2f;
    [SerializeField] private float shakeFrequency = 2f;

    private float shakeTimer;
    private CinemachineBasicMultiChannelPerlin perlin;

    protected override void Awake()
    {
        base.Awake();
        if (virtualCam != null)
        {
            perlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                StopShake();
            }
        }
    }

    public void Shake()
    {
        if (perlin == null) return;

        perlin.m_AmplitudeGain = shakeAmplitude;
        perlin.m_FrequencyGain = shakeFrequency;
        shakeTimer = shakeDuration;
    }

    private void StopShake()
    {
        if (perlin == null) return;

        perlin.m_AmplitudeGain = 0;
        perlin.m_FrequencyGain = 0;
    }
}
