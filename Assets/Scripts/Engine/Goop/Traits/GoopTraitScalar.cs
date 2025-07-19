using GoopGame.Data;
using UnityEngine;

namespace GoopGame.Engine
{
    /// <summary>
    /// Float implementation of <seealso cref="GoopTrait{T}"/>
    /// </summary>
    public class GoopTraitScalar : GoopTrait<float>
    {
        public GoopTraitScalar(GoopTraitDataScalar data)
        {
            TraitData = data;
            Value = GenerateInitialValue();
        }

        public GoopTraitScalar(GoopTraitDataScalar data, float value)
        {
            TraitData = data;
            Value = value;
        }

        protected override float GenerateInitialValue()
        {
            return TraitData.GenerateRandomValue();
        }

        public override float GenerateCombineValue(Goop goop1, Goop goop2)
        {
            //Get weights
            GoopWeightStruct struct1 = GoopTraits.GetWeightStruct(goop1);
            GoopWeightStruct struct2 = GoopTraits.GetWeightStruct(goop2);

            //Get float from other goop
            float value2 = struct2.GetFloat(Type);

            return TraitData.GenerateCombineValue(Value, value2, struct1, struct2);
        }

        public override (float, float) GenerateSplitValue(Goop goop)
        {
            return TraitData.GenerateSplitValues(
                Value,
                GoopTraits.GetWeightStruct(goop)
                );
        }
    }
}
