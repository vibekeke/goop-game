using UnityEngine;
using GoopGame.Data;

namespace GoopGame.Engine
{
    public class GoopTraits
    {
        public GoopTraits(
            GoopTraitDataScalar sizeData,
            GoopTraitDataScalar speedData,
            GoopTraitDataColor colorData
            )
        {
            Size = new GoopTraitScalar(sizeData);
            Speed = new GoopTraitScalar(speedData);
            Color = new GoopTraitColor(colorData);
        }

        public GoopTraits(GoopTraits traits, float size, float speed, Color color)
        {
            Size = new GoopTraitScalar((GoopTraitDataScalar)traits.Size.TraitData, size);
            Speed = new GoopTraitScalar((GoopTraitDataScalar)traits.Speed.TraitData, speed);
            Color = new GoopTraitColor((GoopTraitDataColor)traits.Color.TraitData, color);
        }

        public GoopTraitScalar Size { get; private set; }
        public GoopTraitScalar Speed { get; private set; }
        public GoopTraitColor Color { get; private set; }

        public static GoopWeightStruct GetWeightStruct(Goop goop)
        {
            return new GoopWeightStruct()
            {
                Age = goop.Stats.Age,
                AverageHunger = 1.5f,           //Must be implemented
                AverageTemperature = 1.5f,      //
                AverageMood = 1.5f,             //
                AverageEnergy = 1.5f,           //
                Size = goop.Traits.Size.Value,
                Speed = goop.Traits.Speed.Value,
                Color = goop.Traits.Color.Value
            };
        }

        public static (GoopTraits, GoopTraits) GetGoopTraitsFromSplit(Goop goop)
        {
            (float size1, float size2) = goop.Traits.Size.GenerateSplitValue(goop);
            (float speed1, float speed2) = goop.Traits.Speed.GenerateSplitValue(goop);
            (Color color1, Color color2) = goop.Traits.Color.GenerateSplitValue(goop);

            return (
                new GoopTraits(goop.Traits, size1, speed1, color1),
                new GoopTraits(goop.Traits, size2, speed2, color2)
                );
        }

        public static GoopTraits GetGoopTraitsFromCombine(Goop goop1, Goop goop2)
        {
            float size = goop1.Traits.Size.GenerateCombineValue(goop1, goop2);
            float speed = goop1.Traits.Speed.GenerateCombineValue(goop1, goop2);
            Color color = goop1.Traits.Color.GenerateCombineValue(goop1, goop2);

            return new GoopTraits(goop1.Traits, size, speed, color);
        }
    }
}
