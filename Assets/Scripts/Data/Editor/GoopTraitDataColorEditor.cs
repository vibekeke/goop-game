using UnityEngine;
using UnityEditor;

namespace GoopGame.Data.Editor
{
    /// <summary>
    /// Custom editor for color trait ScriptableObject.
    /// </summary>
    [CustomEditor(typeof(GoopTraitDataColor))]
    public class GoopTraitDataColorEditor : UnityEditor.Editor
    {
        private SerializedProperty _type, _defaultValue, _defaultProbability, _mutationRange,
            _mutationProbability, _parentsLerp, _evolutionWeights;

        //Assign values
        private void OnEnable()
        {
            _type = serializedObject.FindProperty("TraitType");
            _defaultValue = serializedObject.FindProperty("DefaultValue");
            _defaultProbability = serializedObject.FindProperty("DefaultColors");
            _mutationRange = serializedObject.FindProperty("MutationRange");
            _mutationProbability = serializedObject.FindProperty("MutationRangeProbability");
            _parentsLerp = serializedObject.FindProperty("ParentsLerpProbability");
            _evolutionWeights = serializedObject.FindProperty("EvolutionWeights");
        }

        public override void OnInspectorGUI()
        {
            //Fetch the current object
            serializedObject.Update();
            GoopTraitDataColor info = (GoopTraitDataColor)target;

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
            
            //Show default values
            GUILayout.Label("Default value if not generated:");
            EditorGUILayout.PropertyField(_defaultValue, label: GUIContent.none);

            //Show gradient
            GUILayout.Label("Randomly generated default value:");
            EditorGUILayout.PropertyField(_defaultProbability, label: GUIContent.none, GUILayout.Height(60));


            //Show mutation values
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

            //Show parent blend 
            GUILayout.Label("Parent blend probability");
            EditorGUILayout.PropertyField(_parentsLerp, label: GUIContent.none, GUILayout.Height(60));

            EditorGUILayout.Space();

            //Bias list
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
