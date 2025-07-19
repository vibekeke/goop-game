using GoopGame.Utility;
using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// Float implementation of <seealso cref="GoopTraitData{T}"/>
    /// </summary>
    [CreateAssetMenu(
        fileName = "GoopScalarTrait", 
        menuName = "GoopGame/Goops/Traits/Create new Goop Scalar Trait"
        )]
    public class GoopTraitDataScalar : GoopTraitData<float>
    {
        /// <summary>
        /// Lowest default value.
        /// </summary>
        public float DefaultLowerBound;

        /// <summary>
        /// Highest default value.
        /// </summary>
        public float DefaultUpperBound;

        /// <summary>
        /// The probability of generating an initial value within the 
        /// default bounds.
        /// </summary>
        public ProbabilityCurve DefaultBoundsProbability;

        /// <summary>
        /// The maximum mutation range.
        /// </summary>
        public float MutationRange;

        /// <summary>
        /// The probability of generating a value from -MutationRange to 
        /// MutationRange.
        /// </summary>
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

        public override float GenerateCombineValue(
            float value1, float value2, GoopWeightStruct weights1, 
            GoopWeightStruct weights2
            )
        {
            //Calculate parent lerp value, and combine.
            float parentCombinationRatio = 
                ParentsLerpProbability.GetValue(Random.Range(0f, 1f));
            float combinedValue = Mathf.Lerp(
                value1, value2, parentCombinationRatio
                );
            GoopWeightStruct combinedStruct = new GoopWeightStruct(
                weights1, weights2, parentCombinationRatio
                );

            //Calculate biases.
            foreach (var bias in EvolutionWeights)
            {
                if (bias.IsBiasApplied(combinedStruct))
                    ApplyBias(ref combinedValue, bias);
            }

            //Single mutation step.
            DoMutation(ref combinedValue);

            return combinedValue;
        }

        public override (float, float) GenerateSplitValues(
            float value, GoopWeightStruct weights
            )
        {
            //Store values on stack, to be passed by reference.
            float value1 = value;
            float value2 = value;

            //Evaluate biases.
            foreach(var bias in EvolutionWeights)
            {
                if (bias.IsBiasApplied(weights))
                    ApplyBias(ref value1, bias);
                if (bias.IsBiasApplied(weights))
                    ApplyBias(ref value2, bias);
            }

            //Single mutation step.
            DoMutation(ref value1);
            DoMutation(ref value2);

            return (value1, value2);
        }

        public override void ApplyBias(ref float value, EvolutionBias bias)
        {
            //Get delta.
            float delta = bias.BiasTarget - value;
            //Clamp within maximum intensity.
            delta = Mathf.Clamp(
                delta, -bias.MaximumBiasIntensity, bias.MaximumBiasIntensity
                );
            //Multiply delta by weighted random value.
            delta = Mathf.Lerp(
                0f, delta, 
                bias.BiasIntensityProbability.Evaluate(Random.Range(0f, 1f))
                );
            value += delta;
        }

        public override void DoMutation(ref float value)
        {
            //Calculate intensity
            float mutationIntensity = 
                MutationRangeProbability.GetValue(Random.Range(0f, 1f));
            //Calculate mutation
            float mutation = 
                Mathf.Lerp(-MutationRange, MutationRange, mutationIntensity);
            value += mutation;
        }
    }
}
