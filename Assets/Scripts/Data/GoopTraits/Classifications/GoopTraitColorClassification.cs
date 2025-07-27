using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// Color implementation of <seealso cref="GoopTraitClassificationGeneric{T}"/>.
    /// Responsible for determining if a color is within a given range.
    /// </summary>
    [CreateAssetMenu(
        fileName = "Color Classification", 
        menuName = "GoopGame/Goops/Classifications/Color classification")
        ]
    public class GoopTraitColorClassification : 
        GoopTraitClassificationGeneric<Color>
    {
        /// <summary>
        /// The acceptable divergence of a given float.
        /// </summary>
        public float Range;
        public override bool IsInClassification(GoopWeightStruct weights)
        {
            Color value = weights.GetColor(Type);

            float delta = 
                GoopTraitDataColor.ColorMagnitude(Target -  value);

            return delta <= Range;
        }
    }
}
