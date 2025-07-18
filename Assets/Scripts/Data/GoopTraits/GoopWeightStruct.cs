using UnityEngine;

namespace GoopGame.Data
{
    public struct GoopWeightStruct
    {
        public float AverageHunger;
        public float AverageTemperature;
        public float AverageMood;
        public float AverageEnergy;
        public float Age;
        public float Size;
        public float Speed;
        public Color Color;

        public float GetFloat(GoopTraitWeightType type)
        {
            return type switch
            {
                GoopTraitWeightType.AverageHunger => AverageHunger,
                GoopTraitWeightType.AverageTemperature => AverageTemperature,
                GoopTraitWeightType.AverageMood => AverageMood,
                GoopTraitWeightType.AverageEnergy => AverageEnergy,
                GoopTraitWeightType.Age => Age,
                GoopTraitWeightType.Size => Size,
                GoopTraitWeightType.Speed => Speed,
                _ => throw new System.Exception($"Tried to get a float from non-float trait type! {type}")
            };
        }

        public Color GetColor(GoopTraitWeightType type)
        {
            return type switch
            {
                GoopTraitWeightType.Color => Color,
                _ => throw new System.Exception($"Tried to get a color from non-color trait type! {type}")
            };
        }
    }
}
