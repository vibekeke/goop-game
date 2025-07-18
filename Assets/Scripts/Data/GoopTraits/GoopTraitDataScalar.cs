using GoopGame.Utility;
using UnityEngine;

namespace GoopGame.Data
{
    [CreateAssetMenu(fileName = "GoopScalarTrait", menuName = "GoopGame/Goops/Traits/Create new Goop Scalar Trait")]
    public class GoopTraitDataScalar : GoopTraitData<float>
    {
        public float DefaultLowerBound;
        public float DefaultUpperBound;
        public ProbabilityCurve DefaultBoundsProbability;
        public float MutationRange;
        public ProbabilityCurve MutationRangeProbability;

        public override float GenerateRandomValue()
        {
            return Mathf.Lerp(
                DefaultLowerBound, 
                DefaultUpperBound, 
                DefaultBoundsProbability.GetValue(
                    Random.Range(0f,1f)
                    )
                );
        }

        public override float GenerateCombineValue(float value1, float value2, GoopWeightStruct weights1, GoopWeightStruct weights2)
        {
            float parentCombinationRatio = ParentsLerpProbability.GetValue(Random.Range(0f, 1f));
            float combinedValue = Mathf.Lerp(value1, value2, parentCombinationRatio);
            GoopWeightStruct combinedStruct = new GoopWeightStruct(weights1, weights2, parentCombinationRatio);

            foreach (var bias in EvolutionWeights)
            {
                if (bias.IsBiasApplied(combinedStruct))
                    ApplyBias(ref combinedValue, bias);
            }

            DoMutation(ref combinedValue);

            return combinedValue;
        }

        public override (float, float) GenerateSplitValues(float value, GoopWeightStruct weights)
        {
            float value1 = value;
            float value2 = value;

            foreach(var bias in EvolutionWeights)
            {
                if (bias.IsBiasApplied(weights))
                    ApplyBias(ref value1, bias);
                if (bias.IsBiasApplied(weights))
                    ApplyBias(ref value2, bias);
            }

            DoMutation(ref value1);
            DoMutation(ref value2);

            return (value1, value2);
        }

        public override void ApplyBias(ref float value, EvolutionBias bias)
        {
            float delta = bias.BiasTarget - value;
            delta = Mathf.Clamp(delta, -bias.MaximumBiasIntensity, bias.MaximumBiasIntensity);
            delta = Mathf.Lerp(0f, delta, bias.BiasIntensityProbability.Evaluate(Random.Range(0f, 1f)));
            value += delta;
        }

        public override void DoMutation(ref float value)
        {
            float mutationIntensity = MutationRangeProbability.GetValue(Random.Range(0f, 1f));
            float mutation = Mathf.Lerp(0f, MutationRange, mutationIntensity);
            value += mutation;
        }
    }
}
