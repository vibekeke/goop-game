using System;
using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// Color implementation of <seealso cref="GoopTraitEvolutionWeight{T}"/>
    /// </summary>
    [Serializable]
    [CreateAssetMenu(
        fileName = "GoopTraitColorWeight", 
        menuName = "GoopGame/Goops/Traits/Create new Trait Color Weight"
        )]
    public class GoopTraitEvolutionColorWeight : 
        GoopTraitEvolutionWeight<Color>
    {
        public override float GetWeight(Color origin)
        {
            //Get delta
            Color delta = Target - origin;

            return TargetClosenessToProbability.Evaluate(
                1 - Mathf.Clamp01(
                    GoopTraitDataColor.ColorMagnitude(delta) / MaxDistance)
                );
        }
    }
}
