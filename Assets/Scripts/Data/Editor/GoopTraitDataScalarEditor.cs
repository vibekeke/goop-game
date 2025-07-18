using UnityEngine;
using UnityEditor;

namespace GoopGame.Data.Editor
{
    [CustomEditor(typeof(GoopTraitDataScalar))]
    public class GoopTraitDataScalarEditor : UnityEditor.Editor
    {
        private SerializedProperty _type, _defaultValue, _defaultLower, _defaultUpper,
            _defaultProbability, _mutationRange, _mutationProbability, _parentsLerp,
            _evolutionWeights;

        private void OnEnable()
        {
            _type = serializedObject.FindProperty("TraitType");
            _defaultValue = serializedObject.FindProperty("DefaultValue");
            _defaultLower = serializedObject.FindProperty("DefaultLowerBound");
            _defaultUpper = serializedObject.FindProperty("DefaultUpperBound");
            _defaultProbability = serializedObject.FindProperty("DefaultBoundsProbability");
            _mutationRange = serializedObject.FindProperty("MutationRange");
            _mutationProbability = serializedObject.FindProperty("MutationRangeProbability");
            _parentsLerp = serializedObject.FindProperty("ParentsLerpProbability");
            _evolutionWeights = serializedObject.FindProperty("EvolutionWeights");
        }

        public override void OnInspectorGUI()
        {
            //Fetch the current object
            serializedObject.Update();
            GoopTraitDataScalar info = (GoopTraitDataScalar)target;

            //Type display
            EditorGUILayout.PropertyField(_type);

            //Exit if not a valid type
#pragma warning disable CS0612 // Type or member is obsolete
            if ((_type.enumValueFlag & (int)GoopTraitWeightType.IsTrait) == 0)
            {
                EditorGUILayout.LabelField("Trait data cannot be a non-trait type!");
                serializedObject.ApplyModifiedProperties();
                return;
            }
#pragma warning restore CS0612 // Type or member is obsolete

            EditorGUILayout.Space();
            GUILayout.Label("Default values");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Lowest Default");
            _defaultLower.floatValue = EditorGUILayout.FloatField(_defaultLower.floatValue);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Default");
            _defaultValue.floatValue = EditorGUILayout.FloatField(_defaultValue.floatValue);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Highest Default");
            _defaultUpper.floatValue = EditorGUILayout.FloatField(_defaultUpper.floatValue);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Default value probability within bounds");
            EditorGUILayout.PropertyField(_defaultProbability, label: GUIContent.none, GUILayout.Height(60));

            EditorGUILayout.Space();
            GUILayout.Label("Mutation");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Maximum mutation range");
            _mutationRange.floatValue = EditorGUILayout.FloatField(_mutationRange.floatValue);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Mutation intensity probability\n(probability of 0 to Maximum range)");
            EditorGUILayout.PropertyField(_mutationProbability, label: GUIContent.none, GUILayout.Height(60));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.Label("Parent blend probability");
            EditorGUILayout.PropertyField(_parentsLerp, label: GUIContent.none, GUILayout.Height(60));

            EditorGUILayout.Space();

            GUILayout.Label("Evolution bias:\n" +
                "If bias is active add a value after mutation is calculated.\n" +
                "The value is the delta between current value and Bias Target,\n" +
                "The value is then clamped within Maximum Bias Intensity,\n" +
                "The final value is then determined based on the Bias Intensity Probability,\n" +
                "scaled up/down to the clamped value.");
            GUILayout.Label("The product of all the weights determines the probability\n" +
                "of the bias being active or not.");
            EditorGUILayout.PropertyField(_evolutionWeights, label: GUIContent.none);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
