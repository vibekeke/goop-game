using GoopGame.Data;
using UnityEngine;

namespace GoopGame.Engine
{
    public class GoopTraitScalar : GoopTrait<float>
    {
        public GoopTraitScalar(GoopTraitDataScalar data)
        {
            TraitData = data;
        }

        private float _value;
        public override float Value => _value;

        public override float GenerateInitialValue()
        {
            return TraitData.GenerateRandomValue();
        }

        public override float GenerateCombineValue(Goop goop1, Goop goop2)
        {
            GoopWeightStruct struct1 = GoopTraits.GetWeightStruct(goop1);
            GoopWeightStruct struct2 = GoopTraits.GetWeightStruct(goop2);

            return 0f;
        }

        public override (float, float) GenerateSplitValue(Goop goop)
        {
            return (0f, 0f);
        }
    }
}
