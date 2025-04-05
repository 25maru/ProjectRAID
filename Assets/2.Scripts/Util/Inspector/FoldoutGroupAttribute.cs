using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class FoldoutGroupAttribute : PropertyAttribute
{
    public string GroupName;
    public bool GroupAllFieldsUntilNext;
    public int GroupColorIndex;
    public bool ClosedByDefault;

    public FoldoutGroupAttribute(string groupName, bool groupAllFieldsUntilNext = false, int groupColorIndex = 0, bool closedByDefault = false)
    {
        GroupName = groupName;
        GroupAllFieldsUntilNext = groupAllFieldsUntilNext;
        GroupColorIndex = groupColorIndex;
        ClosedByDefault = closedByDefault;
    }
}