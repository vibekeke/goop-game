using UnityEditor;
using UnityEngine;

namespace GoopGame.Utility.Editor
{
    [CustomPropertyDrawer(typeof(ProbabilityCurve))]
    public class ProbabilityCurveDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.PropertyField(position, property.FindPropertyRelative("Curve"), GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
