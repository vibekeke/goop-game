using UnityEditor;

namespace GoopGame.Data.Editor
{
    [CustomEditor(typeof(GoopStatData))]
    public class GoopStatEditor : UnityEditor.Editor
    {
        private SerializedProperty _default, _min, _max, _lessThan, _greaterThan, _type;

        private void OnEnable()
        {
            _default = serializedObject.FindProperty("DefaultValue");
            _min = serializedObject.FindProperty("MinimumValue");
            _max = serializedObject.FindProperty("MaximumValue");
            _lessThan = serializedObject.FindProperty("LessThanThreshold");
            _greaterThan = serializedObject.FindProperty("GreaterThanThreshold");
            _type = serializedObject.FindProperty("StatType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GoopStatData info = (GoopStatData)target;

            EditorGUILayout.PropertyField(_type);

            EditorGUILayout.PropertyField(_default);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Minimum Value");
            EditorGUILayout.LabelField("Maximum Value");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _min.floatValue = EditorGUILayout.FloatField(_min.floatValue);
            _max.floatValue = EditorGUILayout.FloatField(_max.floatValue);
            EditorGUILayout.EndHorizontal();

            string lessThanText = (GoopStatTypes)_type.enumValueIndex switch
            {
                GoopStatTypes.Hunger => "Hungry",
                GoopStatTypes.Temperature => "Cold",
                GoopStatTypes.Mood => "Sad",
                GoopStatTypes.Energy => "Tired",
                _ => "Low State"
            };
            string greaterThanText = (GoopStatTypes)_type.enumValueIndex switch
            {
                GoopStatTypes.Hunger => "Sated",
                GoopStatTypes.Temperature => "Hot",
                GoopStatTypes.Mood => "Happy",
                GoopStatTypes.Energy => "Energized",
                _ => "High State"
            };

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{lessThanText} if less than:");
            EditorGUILayout.LabelField($"{greaterThanText} if greater than:");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _lessThan.floatValue = EditorGUILayout.FloatField(_lessThan.floatValue);
            _greaterThan.floatValue = EditorGUILayout.FloatField(_greaterThan.floatValue);
            EditorGUILayout.EndHorizontal();

            if (_lessThan.floatValue > _greaterThan.floatValue)
                EditorGUILayout.LabelField("Error: LessThanValue > GreaterThanValue!");
            if (_lessThan.floatValue < _min.floatValue)
                EditorGUILayout.LabelField("Error: LessThanValue < MinimumValue!");
            if (_greaterThan.floatValue > _max.floatValue)
                EditorGUILayout.LabelField("Error: GreaterThanValue > MaximumValue!");
            if (_min.floatValue > _max.floatValue)
                EditorGUILayout.LabelField("Error: MinimumValue > MaximumValue!");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
