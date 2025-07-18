using System;
using UnityEngine;

namespace GoopGame.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "GoopTraitColorWeight", menuName = "GoopGame/Goops/Traits/Create new Trait Color Weight")]
    public class GoopTraitEvolutionColorWeight : GoopTraitEvolutionWeight<Color>
    {
        public override float GetWeight(Color origin)
        {
            Vector3 originVector = new Vector3(origin.r, origin.g, origin.b);
            Vector3 targetVector = new Vector3(Target.r, Target.g, Target.b);

            return TargetClosenessToProbability.Evaluate(
                1 - Mathf.Clamp01((targetVector - originVector).magnitude / MaxDistance)
                );
        }
    }
}
