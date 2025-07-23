using GoopGame.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// An abstract generic ScriptableObject where <typeparamref name="T"/>
    /// is the data type of the trait.
    /// Responsible for storing data pertaining to how it generates and
    /// mutates.
    /// </summary>
    public abstract class GoopTraitData<T> : ScriptableObject
    {
        /// <summary>
        /// The default value of this trait
        /// Should not be used.
        /// </summary>
        public T DefaultValue;

        /// <summary>
        /// The WeightType of this trait.
        /// </summary>
        public GoopTraitWeightType TraitType;

        /// <summary>
        /// How likely each value from 0-1 is when choosing how to lerp
        /// two goops' values on combine.
        /// </summary>
        public ProbabilityCurve ParentsLerpProbability;

        /// <summary>
        /// Generates a random initial value for this trait.
        /// </summary>
        public abstract T GenerateRandomValue();

        /// <summary>
        /// Generates two split values based on a value and a given 
        /// weight struct.
        /// </summary>
        public abstract (T, T) GenerateSplitValues(T value, 
            GoopWeightStruct weights
            );

        /// <summary>
        /// Generates a value based on two values and weight structs.
        /// </summary>
        public abstract T GenerateCombineValue(T value1, T value2, 
            GoopWeightStruct weights1, GoopWeightStruct weights2
            );

        /// <summary>
        /// A serialized (Unity inspector requirement) struct to store
        /// biases to evaluate during the generation of new values.
        /// Provides a target to evolve towards, information about how 
        /// much to evolve, and a list of weights to determine if the 
        /// bias takes place or not.
        /// </summary>
        [Serializable]
        public struct EvolutionBias
        {
            /// <summary>
            /// The target of the bias.
            /// </summary>
            [Tooltip("The target of the bias.")]
            public T BiasTarget;
            /// <summary>
            /// The maximum distance this bias can apply.
            /// </summary>
            [Tooltip("The maximum distance this bias can apply.")]
            public float MaximumBiasIntensity;
            /// <summary>
            /// The probability of distance to apply from 0 - MaximumBias.
            /// </summary>
            [Tooltip("The probability of distance to apply from " +
                "0 - MaximumBias")]
            public AnimationCurve BiasIntensityProbability;
            /// <summary>
            /// Weights from 0 - 1 are multiplied to determine the
            /// probability of if the bias should apply
            /// </summary>
            [Tooltip("Weights from 0 - 1 are multiplied to determine " +
                "probability of if the bias should apply.")]
            public List<TypeWeightPair> Weights;

            /// <summary>
            /// Checks if bias is applied based on a probability generated
            /// from the product of its weights.
            /// </summary>
            public bool IsBiasApplied(GoopWeightStruct weightStruct)
            {
                //probability base is 1.
                float probability = 1f;

                //Check all weights.
                foreach (var weight in Weights)
                {
                    //Multiply current probability based on weight.
                    probability *= weight.GetWeight(weightStruct);
                }

                //Decide based on final probability.
                return probability > UnityEngine.Random.Range(0f, 1f);
            }
        }

        /// <summary>
        /// The biases present to this trait.
        /// </summary>
        [Tooltip("If a bias is applied, the probability of which is " +
            "determined by its weights, it overwrites any inherent " +
            "evolution variance.")]
        public List<EvolutionBias> EvolutionWeights;

        /// <summary>
        /// Applies a bias to a value passed by reference.
        /// </summary>
        /// <param name="value">The value to modify by reference.</param>
        public abstract void ApplyBias(ref T value, EvolutionBias bias);
        /// <summary>
        /// Does a single round of mutation to a value passed by referene.
        /// </summary>
        /// <param name="value">The value to modify by reference.</param>
        public abstract void DoMutation(ref T value);
    }
}
