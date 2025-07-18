using GoopGame.Utility;
using UnityEngine;

namespace GoopGame.Data
{
    [CreateAssetMenu(fileName = "GoopColorTrait", menuName = "GoopGame/Goops/Traits/Create new Goop Color Trait")]
    public class GoopTraitDataColor : GoopTraitData<Color>
    {
        public Gradient DefaultColors;
        public float MutationRange;
        public ProbabilityCurve MutationProbability;

        public override Color GenerateRandomValue()
        {
            return DefaultColors.Evaluate(Random.Range(0f, 1f));
        }

        public override Color GenerateCombineValue(Color value1, Color value2, GoopWeightStruct weights1, GoopWeightStruct weights2)
        {
            throw new System.NotImplementedException();
        }

        public override (Color, Color) GenerateSplitValues(Color value, GoopWeightStruct weights)
        {
            throw new System.NotImplementedException();
        }
    }
}
