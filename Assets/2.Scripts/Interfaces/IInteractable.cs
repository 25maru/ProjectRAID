/// <summary>
/// 플레이어와 상호작용 가능한 오브젝트가 구현할 인터페이스
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// 상호작용 실행하고, 상호작용을 요청한 플레이어를 전달
    /// </summary>
    /// <param name="player">상호작용을 시도한 플레이어</param>
    void Interact(PlayerController player);

    /// <summary>
    /// 상호작용 힌트나 UI를 띄우고 싶을 때 호출 (예: Outline 효과)
    /// </summary>
    void ShowHighlight();

    /// <summary>
    /// 상호작용 가능 오브젝트에서 멀어졌을 때 호출
    /// </summary>
    void HideHighlight();
}
