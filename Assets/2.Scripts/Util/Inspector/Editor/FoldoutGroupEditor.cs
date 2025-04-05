using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class FoldoutGroupEditor : Editor
{
    [SerializeField] private StyleSheet editorStyleSheet;

    private Dictionary<string, List<SerializedProperty>> groupedProperties;
    private Dictionary<string, FoldoutGroupAttribute> groupAttributes;
    private List<SerializedProperty> ungroupedProperties;

    public override VisualElement CreateInspectorGUI()
    {
        // 그룹, 속성 리스트 초기화
        groupedProperties = new Dictionary<string, List<SerializedProperty>>();
        groupAttributes = new Dictionary<string, FoldoutGroupAttribute>();
        ungroupedProperties = new List<SerializedProperty>();

        var root = new VisualElement();

        // 스타일시트 적용
        if (editorStyleSheet != null)
        {
            root.styleSheets.Add(editorStyleSheet);
        }

        var iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); // m_Script 필드 건너뛰기

        // m_Script 필드 포함시키기 (스크립트 연결 필드 표시)
        var scriptField = new PropertyField(iterator.Copy()) { name = "Script" };
        scriptField.SetEnabled(false);
        root.Add(scriptField);

        var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FoldoutGroupAttribute currentGroup = null;

        // 속성 순회하면서 그룹 속성 분석
        while (iterator.NextVisible(false))
        {
            var field = Array.Find(fields, f => f.Name == iterator.name);
            var groupAttr = field?.GetCustomAttribute<FoldoutGroupAttribute>();

            if (groupAttr != null)
            {
                currentGroup = groupAttr;
                AddToGroup(groupAttr.GroupName, groupAttr, iterator);
                continue;
            }

            if (currentGroup != null && currentGroup.GroupAllFieldsUntilNext)
            {
                AddToGroup(currentGroup.GroupName, currentGroup, iterator);
            }
            else
            {
                ungroupedProperties.Add(iterator.Copy());
                currentGroup = null;
            }
        }

        // FoldoutGroup이 하나도 없으면 기본 인스펙터 표시
        if (groupedProperties.Count == 0)
        {
            var defaultInspector = new VisualElement();
            InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
            return defaultInspector;
        }

        // 그룹된 속성들 렌더링
        foreach (var groupName in groupedProperties.Keys)
        {
            var foldout = new Foldout
            {
                text = groupName,
                value = !groupAttributes[groupName].ClosedByDefault
            };

            foldout.style.borderLeftWidth = 4;
            foldout.style.borderLeftColor = ExtendedColors.GetColorAt(groupAttributes[groupName].GroupColorIndex);
            foldout.style.marginBottom = 6;
            foldout.style.paddingLeft = 4;

            foreach (var prop in groupedProperties[groupName])
            {
                foldout.Add(new PropertyField(prop));
            }

            root.Add(foldout);
        }

        // 그룹되지 않은 속성들 렌더링
        foreach (var prop in ungroupedProperties)
        {
            root.Add(new PropertyField(prop));
        }

        return root;
    }

    // 그룹에 속성 추가
    private void AddToGroup(string name, FoldoutGroupAttribute attr, SerializedProperty prop)
    {
        if (!groupedProperties.ContainsKey(name))
        {
            groupedProperties[name] = new List<SerializedProperty>();
            groupAttributes[name] = attr;
        }
        groupedProperties[name].Add(prop.Copy());
    }
}