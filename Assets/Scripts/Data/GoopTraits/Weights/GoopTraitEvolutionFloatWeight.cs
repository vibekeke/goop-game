using System;
using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// Float implementation of <seealso cref="GoopTraitEvolutionWeight{T}"/>
    /// </summary>
    [Serializable]
    [CreateAssetMenu(
        fileName = "GoopTraitFloatWeight",
        menuName = "GoopGame/Goops/Traits/Create new Trait Float Weight"
        )]
    public class GoopTraitEvolutionFloatWeight : 
        GoopTraitEvolutionWeight<float>
    {
        public override float GetWeight(float origin)
        {
            return TargetClosenessToProbability.Evaluate(
                1 - Mathf.Clamp01(Mathf.Abs(Target - origin) / MaxDistance)
                );
        }
    }
}
