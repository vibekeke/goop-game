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

        public GoopWeightStruct(GoopWeightStruct struct1, 
            GoopWeightStruct struct2, float ratio
            )
        {
            AverageHunger = Mathf.Lerp(
                struct1.AverageHunger, struct2.AverageHunger, ratio
                );
            AverageTemperature = Mathf.Lerp(
                struct1.AverageTemperature, struct2.AverageTemperature, ratio
                );
            AverageMood = Mathf.Lerp(
                struct1.AverageMood, struct2.AverageMood, ratio
                );
            AverageEnergy = Mathf.Lerp(
                struct1.AverageEnergy, struct2.AverageEnergy, ratio
                );
            Age = Mathf.Lerp(struct1.Age, struct2.Age, ratio);
            Size = Mathf.Lerp(struct1.Size, struct2.Size, ratio);
            Speed = Mathf.Lerp(struct1.Speed, struct2.Speed, ratio);
            Color = Color.Lerp(struct1.Color, struct2.Color, ratio);
        }

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
                _ => throw new System.Exception(
                    $"Tried to get a float from non-float trait type! {type}"
                    )
            };
        }

        public Color GetColor(GoopTraitWeightType type)
        {
            return type switch
            {
                GoopTraitWeightType.Color => Color,
                _ => throw new System.Exception(
                    $"Tried to get a color from non-color trait type! {type}"
                    )
            };
        }
    }
}
