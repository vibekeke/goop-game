using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// An abstract generic class used to determine if a trait of datatype.
    /// Is a non-generic so that it can be stored in regular C# collections.
    /// <seealso cref="GoopTraitClassificationGeneric{T}"/> contains generic
    /// value fields.
    /// </summary>
    public abstract class GoopTraitClassification : ScriptableObject
    {
        /// <summary>
        /// Returns a bool corresponding to if a given value falls within
        /// this classification.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>If the evaluated value falls within the 
        /// classification.</returns>
        public abstract bool IsInClassification(GoopWeightStruct weights);
    }
}
