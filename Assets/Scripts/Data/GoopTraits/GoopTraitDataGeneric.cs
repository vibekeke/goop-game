using GoopGame.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoopGame.Data
{
    [Serializable]
    public class TypeWeightStruct
    {
        public GoopTraitWeightType Type;
        public GoopTraitEvolutionFloatWeight FloatWeight;
        public GoopTraitEvolutionColorWeight ColorWeight;

        public float GetWeight(GoopWeightStruct weight)
        {
#pragma warning disable CS0612 // Type or member is obsolete
            if ((Type & GoopTraitWeightType.IsFloat) != 0)
                return FloatWeight.GetWeight(weight.GetFloat(Type));
            else if ((Type & GoopTraitWeightType.IsColor) != 0)
                return ColorWeight.GetWeight(weight.GetColor(Type));
            else
                throw new NotImplementedException($"Tried to get weight from non-implemented weight datatype! {Type}");
#pragma warning restore CS0612 // Type or member is obsolete
        }
    }

    public abstract class GoopTraitData<T> : ScriptableObject
    {
        public T DefaultValue;
        public GoopTraitWeightType TraitType;
        public ProbabilityCurve ParentsLerpProbability;
        public abstract T GenerateRandomValue();
        public abstract (T, T) GenerateSplitValues(T value, GoopWeightStruct weights);
        public abstract T GenerateCombineValue(T value1, T value2, GoopWeightStruct weights1, GoopWeightStruct weights2);

        [Serializable]
        public struct EvolutionBias
        {
            [Tooltip("The target of the bias.")]
            public T BiasTarget;
            [Tooltip("The maximum distance this bias can apply.")]
            public float MaximumBiasIntensity;
            [Tooltip("The probability of distance to apply from 0 - MaximumBias")]
            public AnimationCurve BiasIntensityProbability;
            [Tooltip("Weights from 0 - 1 are multiplied to determine probability of if the bias should apply.")]
            public List<TypeWeightStruct> Weights;

            public bool IsBiasApplied(GoopWeightStruct weightStruct)
            {
                float probability = 1f;
                foreach (var weight in Weights)
                {
                    probability *= weight.GetWeight(weightStruct);
                }
                return probability > UnityEngine.Random.Range(0f, 1f);
            }
        }

        [Tooltip("If a bias is applied, the probability of which is determined by its weights, it overwrites any inherent evolution variance.")]
        public List<EvolutionBias> EvolutionWeights;

        public abstract void ApplyBias(ref T value, EvolutionBias bias);
        public abstract void DoMutation(ref T value);
    }
}
