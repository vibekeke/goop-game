using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// An abstract generic class used to determine if a trait of datatype
    /// T is within a given range.
    /// Inherits from a non-generic class 
    /// (<seealso cref="GoopTraitClassification"/>) so that it can be
    /// stored in regular C# collections.
    /// </summary>
    public abstract class GoopTraitClassificationGeneric<T> : 
        GoopTraitClassification
    {
        /// <summary>
        /// The specific trait this classification evaluates.
        /// </summary>
        public GoopTraitWeightType Type;
        /// <summary>
        /// The ideal value of this classification.
        /// Used in <seealso cref="IsInClassification(T)"/>.
        /// </summary>
        public T Target;
    }
}
