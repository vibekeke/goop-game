using UnityEditor;
using UnityEngine;

namespace GoopGame.Data.Editor
{
    [CustomPropertyDrawer(typeof(TypeWeightStruct))]
    public class TypeWeightStructDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty typeProperty = property.FindPropertyRelative("Type");
            SerializedProperty weightProperty;
            GoopTraitWeightType flag = (GoopTraitWeightType)typeProperty.enumValueFlag;
#pragma warning disable CS0612 // Type or member is obsolete
            if ((GoopTraitWeightType.IsColor & flag) != 0)
                weightProperty = property.FindPropertyRelative("ColorWeight");
            else if ((GoopTraitWeightType.IsFloat & flag) != 0)
                weightProperty = property.FindPropertyRelative("FloatWeight");
            else if (flag == GoopTraitWeightType.None)
            {
                EditorGUI.PropertyField(position, typeProperty, GUIContent.none);
                EditorGUI.EndProperty();
                return;
            }
            else
            {
                EditorGUI.PropertyField(position, typeProperty, GUIContent.none);
                EditorGUI.EndProperty();
                Debug.LogError($"No custom property drawer defined for type index: {typeProperty.enumValueIndex}!");
                return;
            }
#pragma warning restore CS0612 // Type or member is obsolete

            // Calculate rects
            var rect1 = new Rect(position.x, position.y, position.width * 0.5f, position.height);
            var rect2 = new Rect(rect1.max.x, rect1.position.y, rect1.width, rect1.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(rect1, typeProperty, GUIContent.none);
            EditorGUI.PropertyField(rect2, weightProperty, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}
