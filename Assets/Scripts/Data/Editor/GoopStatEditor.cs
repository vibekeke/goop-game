using UnityEditor;

namespace GoopGame.Data.Editor
{
    /// <summary>
    /// Custom editor window for GoopStatData ScriptableObjects.
    /// </summary>
    [CustomEditor(typeof(GoopStatData))]
    public class GoopStatEditor : UnityEditor.Editor
    {
        /// <summary>
        /// GoopStatData properties.
        /// </summary>
        private SerializedProperty _default, _min, _max, _lessThan, _greaterThan, _type;

        /// <summary>
        /// Initialize the properties with string identifiers.
        /// !!! Changing the variable names in GoopStatEditor breaks this.
        /// </summary>
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
            //Fetch the current object
            serializedObject.Update();
            GoopStatData info = (GoopStatData)target;

            //Type display
            EditorGUILayout.PropertyField(_type);

            //Default value display
            EditorGUILayout.PropertyField(_default);

            //Show minimum and maximum values
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Minimum Value");
            EditorGUILayout.LabelField("Maximum Value");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _min.floatValue = EditorGUILayout.FloatField(_min.floatValue);
            _max.floatValue = EditorGUILayout.FloatField(_max.floatValue);
            EditorGUILayout.EndHorizontal();

            //Set the threshold texts to match the stat type.
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

            //Show threshold values.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{lessThanText} if less than:");
            EditorGUILayout.LabelField($"{greaterThanText} if greater than:");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _lessThan.floatValue = EditorGUILayout.FloatField(_lessThan.floatValue);
            _greaterThan.floatValue = EditorGUILayout.FloatField(_greaterThan.floatValue);
            EditorGUILayout.EndHorizontal();

            //Show potential errors.
            if (_lessThan.floatValue > _greaterThan.floatValue)
                EditorGUILayout.LabelField("Error: LessThanValue > GreaterThanValue!");
            if (_lessThan.floatValue < _min.floatValue)
                EditorGUILayout.LabelField("Error: LessThanValue < MinimumValue!");
            if (_greaterThan.floatValue > _max.floatValue)
                EditorGUILayout.LabelField("Error: GreaterThanValue > MaximumValue!");
            if (_min.floatValue > _max.floatValue)
                EditorGUILayout.LabelField("Error: MinimumValue > MaximumValue!");

            //Apply modified properties, otherwise the scriptable object won't actually be written to.
            serializedObject.ApplyModifiedProperties();
        }
    }
}
