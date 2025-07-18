using GoopGame.Utility;
using UnityEngine;

namespace GoopGame.Data
{
    [CreateAssetMenu(fileName = "GoopScalarTrait", menuName = "GoopGame/Goops/Traits/Create new Goop Scalar Trait")]
    public class GoopTraitDataScalar : GoopTraitData<float>
    {
        public float DefaultLowerBound;
        public float DefaultUpperBound;
        public ProbabilityCurve DefaultBoundsProbability;
        public float MutationRange;
        public ProbabilityCurve MutationRangeProbability;
        public ProbabilityCurve ParentsLerpProbability;

        public override float GenerateRandomValue()
        {
            throw new System.NotImplementedException();
        }

        public override (float, float) GenerateSplitValues(float value, GoopWeightStruct weights)
        {
            throw new System.NotImplementedException();
        }

        public override float GenerateCombineValue(float value1, float value2, GoopWeightStruct weights1, GoopWeightStruct weights2)
        {
            throw new System.NotImplementedException();
        }
    }
}
