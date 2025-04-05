using UnityEngine;

/// <summary>
/// 인스펙터에서 변수들을 Foldout 그룹으로 묶기 위한 Attribute
/// </summary>
public class FoldoutGroupAttribute : PropertyAttribute
{
    public string groupName;
    public bool groupAllFieldsUntilNext = true;
    public bool closedByDefault = false;
    public int colorIndex = -1;
    public ExtendedColor? colorEnum = null;

    /// <summary>
    /// 색상 인덱스로 Foldout 그룹 설정
    /// </summary>
    public FoldoutGroupAttribute(string groupName, int colorIndex = 50, bool groupAllFieldsUntilNext = true, bool closedByDefault = false)
    {
        this.groupName = groupName;
        this.colorIndex = colorIndex;
        this.groupAllFieldsUntilNext = groupAllFieldsUntilNext;
        this.closedByDefault = closedByDefault;
    }

    /// <summary>
    /// 색상 enum으로 Foldout 그룹 설정
    /// </summary>
    public FoldoutGroupAttribute(string groupName, ExtendedColor colorEnum, bool groupAllFieldsUntilNext = true, bool closedByDefault = false)
    {
        this.groupName = groupName;
        this.colorEnum = colorEnum;
        this.groupAllFieldsUntilNext = groupAllFieldsUntilNext;
        this.closedByDefault = closedByDefault;
    }
}