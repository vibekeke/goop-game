using System;
using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// An abstract generic ScriptableObject to store evolution weight 
    /// data of type <typeparamref name="T"/>. It generates a delta by 
    /// comparing a value to Target, divides the delta by MaxDistance, 
    /// and fetches probability from a curve at x = clamped delta clamped 
    /// to 0-1 range.
    /// </summary>
    [Serializable]
    public abstract class GoopTraitEvolutionWeight<T> : ScriptableObject
    {
        /// <summary>
        /// The value to find a "distance from".
        /// </summary>
        public T Target;
        /// <summary>
        /// At what point the value-Target delta is maxed out. Delta is
        /// divided by this value before being clamped to 0-1 range.
        /// </summary>
        public float MaxDistance;
        /// <summary>
        /// The curve that determines the probability of this weight.
        /// The point of the curve that is chosen is x = (Target -
        /// origin) divided by MaxDistance and clamped to 0-1 range.
        /// </summary>
        public AnimationCurve TargetClosenessToProbability;
        /// <summary>
        /// Returns the probability of this weight. Calculated by taking
        /// a point on this weight's curve, where x = (Target - 
        /// <paramref name="origin"/>) divided by MaxDistance and clamped
        /// to 0-1 range.
        /// </summary>
        /// <param name="origin">The value to compare</param>
        /// <returns>The probability of this weight</returns>
        public abstract float GetWeight(T origin);
    }
}
