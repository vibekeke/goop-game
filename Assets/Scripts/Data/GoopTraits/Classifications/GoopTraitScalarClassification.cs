using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// Float implementation of <seealso cref="GoopTraitClassificationGeneric{T}"/>.
    /// Responsible for determining if a float is within a given range.
    /// </summary>
    [CreateAssetMenu(
        fileName = "Scalar Classification",
        menuName = "GoopGame/Goops/Classifications/Scalar classification")
        ]
    public class GoopTraitScalarClassification : 
        GoopTraitClassificationGeneric<float>
    {
        /// <summary>
        /// The acceptable divergence of a given float.
        /// </summary>
        public float Range;
        public override bool IsInClassification(GoopWeightStruct weights)
        {
            float value = weights.GetFloat(Type);

            return Mathf.Abs(Target - value) <= Range;
        }
    }
}
