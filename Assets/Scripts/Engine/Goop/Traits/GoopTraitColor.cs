using GoopGame.Data;
using UnityEngine;

namespace GoopGame.Engine
{
    public class GoopTraitColor : GoopTrait<Color>
    {
        public GoopTraitColor(GoopTraitDataColor data)
        {
            TraitData = data;
            Value = GenerateInitialValue();
        }

        public GoopTraitColor(GoopTraitDataColor data, Color value)
        {
            TraitData = data;
            Value = value;
        }

        protected override Color GenerateInitialValue()
        {
            return TraitData.GenerateRandomValue();
        }

        public override Color GenerateCombineValue(Goop goop1, Goop goop2)
        {
            GoopWeightStruct struct1 = GoopTraits.GetWeightStruct(goop1);
            GoopWeightStruct struct2 = GoopTraits.GetWeightStruct(goop2);

            Color value2 = struct2.GetColor(Type);

            return TraitData.GenerateCombineValue(Value, value2, struct1, struct2);
        }

        public override (Color, Color) GenerateSplitValue(Goop goop)
        {
            return TraitData.GenerateSplitValues(
                Value,
                GoopTraits.GetWeightStruct(goop)
                );
        }
    }
}
