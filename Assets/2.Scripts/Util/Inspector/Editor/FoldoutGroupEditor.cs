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
        groupedProperties = new();
        groupAttributes = new();
        ungroupedProperties = new();

        var root = new VisualElement();

        if (editorStyleSheet != null)
        {
            root.styleSheets.Add(editorStyleSheet);
        }

        var iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);

        var scriptField = new PropertyField(iterator.Copy()) { name = "Script" };
        scriptField.SetEnabled(false);
        root.Add(scriptField);

        var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        FoldoutGroupAttribute currentGroup = null;

        while (iterator.NextVisible(false))
        {
            var field = Array.Find(fields, f => f.Name == iterator.name);
            var groupAttr = field?.GetCustomAttribute<FoldoutGroupAttribute>();

            if (groupAttr != null)
            {
                currentGroup = groupAttr;
                AddToGroup(groupAttr.groupName, groupAttr, iterator);
                continue;
            }

            if (currentGroup != null && currentGroup.groupAllFieldsUntilNext)
            {
                AddToGroup(currentGroup.groupName, currentGroup, iterator);
            }
            else
            {
                ungroupedProperties.Add(iterator.Copy());
                currentGroup = null;
            }
        }

        if (groupedProperties.Count == 0)
        {
            var defaultInspector = new VisualElement();
            InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
            return defaultInspector;
        }

        foreach (var groupName in groupedProperties.Keys)
        {
            var attr = groupAttributes[groupName];
            var foldout = new Foldout
            {
                text = groupName,
                value = !attr.closedByDefault
            };

            Color color = Color.gray;
            if (attr.colorEnum.HasValue)
            {
                var field = typeof(ExtendedColors).GetField(attr.colorEnum.Value.ToString(), BindingFlags.Public | BindingFlags.Static);
                if (field != null && field.FieldType == typeof(Color))
                {
                    color = (Color)field.GetValue(null);
                }
            }
            else
            {
                color = ExtendedColors.GetColorAt(attr.colorIndex);
            }

            foldout.style.borderLeftWidth = 3;
            foldout.style.borderRightWidth = 0;
            foldout.style.borderLeftColor = color;
            foldout.style.borderLeftColor = color;
            foldout.style.marginBottom = 6;
            foldout.style.paddingLeft = 0;
            foldout.style.paddingRight = 0;

            foreach (var prop in groupedProperties[groupName])
            {
                foldout.Add(new PropertyField(prop));
            }

            root.Add(foldout);
        }

        foreach (var prop in ungroupedProperties)
        {
            root.Add(new PropertyField(prop));
        }

        return root;
    }

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
