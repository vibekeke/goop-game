using System;
using UnityEngine;

namespace GoopGame.Data
{
    [Serializable]
    public abstract class GoopTraitEvolutionWeight<T> : ScriptableObject
    {
        public T Target;
        public float MaxDistance;
        public AnimationCurve TargetClosenessToProbability;
        public abstract float GetWeight(T origin);
    }
}
