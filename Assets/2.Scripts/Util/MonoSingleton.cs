using UnityEngine;

/// <summary>
/// MonoBehaviour 대신 상속받아 간편하게 싱글톤을 구현할 수 있습니다.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    private static readonly object lockObject = new();
    private static bool isShuttingDown = false;

    /// <summary>
    /// 해당 싱글톤이 씬 전환 시 파괴되지 않도록 설정할지 여부 (기본값: true)
    /// </summary>
    protected virtual bool IsPersistent => true;

    public static T Instance
    {
        get
        {
            if (isShuttingDown)
            {
                Debug.LogWarning($"[MonoSingleton] {typeof(T)}의 인스턴스가 이미 삭제되었습니다. null을 반환합니다.");
                return null;
            }

            lock (lockObject)
            {
#if UNITY_2023_1_OR_NEWER
                instance ??= FindFirstObjectByType<T>();
#else
                instance ??= FindObjectOfType<T>();
#endif

                if (instance == null)
                {
                    GameObject go = new(typeof(T).Name);
                    instance = go.AddComponent<T>();

                    if (instance.IsPersistent)
                    {
                        DontDestroyOnLoad(go);
                    }
                }

                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (IsPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            isShuttingDown = true;
        }
    }
}
