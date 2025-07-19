using GoopGame.Utility;
using UnityEngine;

namespace GoopGame.Data
{
    [CreateAssetMenu(
        fileName = "GoopColorTrait", 
        menuName = "GoopGame/Goops/Traits/Create new Goop Color Trait"
        )]
    public class GoopTraitDataColor : GoopTraitData<Color>
    {
        public Gradient DefaultColors;
        public float MutationRange;
        public ProbabilityCurve MutationRangeProbability;

        public override Color GenerateRandomValue()
        {
            return DefaultColors.Evaluate(Random.Range(0f, 1f));
        }

        public override Color GenerateCombineValue(Color value1, 
            Color value2, GoopWeightStruct weights1, 
            GoopWeightStruct weights2
            )
        {
            float parentCombinationRatio = 
                ParentsLerpProbability.GetValue(Random.Range(0f, 1f));
            Color delta = 
                NormalizeColor(value2 - value1) * parentCombinationRatio;
            Color combinedValue = value1 + delta;
            GoopWeightStruct combinedStruct = new GoopWeightStruct(
                weights1, weights2, parentCombinationRatio
                );

            foreach (var bias in EvolutionWeights)
            {
                if (bias.IsBiasApplied(combinedStruct))
                    ApplyBias(ref combinedValue, bias);
            }

            DoMutation(ref combinedValue);

            return combinedValue;
        }

        public override (Color, Color) GenerateSplitValues(Color value, 
            GoopWeightStruct weights
            )
        {
            Color value1 = value;
            Color value2 = value;

            foreach (var bias in EvolutionWeights)
            {
                if (bias.IsBiasApplied(weights))
                    ApplyBias(ref value1, bias);
                if (bias.IsBiasApplied(weights))
                    ApplyBias(ref value2, bias);
            }

            DoMutation(ref value1);
            DoMutation(ref value2);

            return (value1, value2);
        }

        public override void ApplyBias(ref Color value, EvolutionBias bias)
        {
            Color delta = ClampColor(bias.BiasTarget - value);
            float magnitude = ColorMagnitude(delta);
            delta = NormalizeColor(delta) * 
                Mathf.Min(magnitude, MutationRange) * 
                bias.BiasIntensityProbability.Evaluate(
                    Random.Range(0f, 1f)
                    );
            value += delta;
        }

        public override void DoMutation(ref Color value)
        {
            float mutationIntensity = MutationRangeProbability.GetValue(
                Random.Range(0f, 1f)
                );
            float mutation = Mathf.Lerp(
                0f, MutationRange, mutationIntensity
                );
            Color target = GenerateRandomValue();

            Color delta = NormalizeColor(target - value);
            value += delta * mutation;
        }

        public static Color ClampColor(Color color)
        {
            return new Color(
                Mathf.Clamp01(color.r),
                Mathf.Clamp01(color.g),
                Mathf.Clamp01(color.b),
                Mathf.Clamp01(color.a)
                );
        }

        public static float ColorMagnitude(Color color)
        {
            float magnitude = color.r * color.r + color.g * color.g +
                color.b * color.b + color.a * color.a;

            return Mathf.Sqrt(magnitude);
        }

        public static Color NormalizeColor(Color color)
        {
            float magnitude = ColorMagnitude(color);

            return new Color(
                color.r / magnitude, color.g / magnitude,
                color.b / magnitude, color.a / magnitude
                );
        }
    }
}
