using System;
using UnityEngine;

namespace GoopGame.Utility
{
    //Created using "Unity Animation Curves for Sampling" by Sebastian Schöner
    //retrieved from https://blog.s-schoener.com/2018-05-05-animation-curves/

    //Moved to an inheriting class to avoid having to initialize each instance
    //separately in code.
    //Additional comments for more context.

    /// <summary>
    /// Samples according to a density given by an animation curve.
    /// This assumes that the animation curve is non-negative everywhere.
    /// </summary>
    [Serializable]
    public class ProbabilityCurve
    {
        private const int integrationSteps = 100;
        //NonSerialized to make sure reference is set to null when testing in editor environment.
        //https://discussions.unity.com/t/scriptableobjects-keep-private-values/825847
        [NonSerialized]
        private IntegrateFunction _integratedDensity;

        public AnimationCurve Curve;

        public float GetValue(float random)
        {
            if (_integratedDensity == null)
                _integratedDensity = new IntegrateFunction(
                    Curve.Evaluate,
                    MinT,
                    MaxT,
                    integrationSteps
                    );

            return Invert(random);
        }

        /// <summary>
        /// Takes a value s in [0, 1], scales it up to the interval
        /// [0, totalIntegratedValue] and computes its inverse.
        /// </summary>
        private float Invert(float s)
        {
            //scales float from 0,1 range to total area of function (integral).
            s *= _integratedDensity.Total;
            float lower = MinT;
            float upper = MaxT;
            //max depth to still calculate (stops when segment smaller than precision)
            const float precision = 0.00001f;
            while (upper - lower > precision)
            {
                //find midpoint of current segment
                float mid = (lower + upper) / 2f;
                //calculate integrated value of midpoint to compare.
                float d = _integratedDensity.Evaluate(mid);
                //if integrated midpoint is greater than scaled float, scaled float 
                //exists in the lower half of the segment. We then set the new upper
                //bound of segment to midpoint before next iteration.
                if (d > s)
                    upper = mid;
                //...and vice versa for if its lower.
                else if (d < s)
                    lower = mid;
                else
                {
                    // unlikely :)
                    // i agree : )
                    return mid;
                }
            }

            //finally return the average of our segment.
            return (lower + upper) / 2f;
        }

        private float MinT => Curve.keys[0].time;
        private float MaxT => Curve.keys[Curve.length - 1].time;
    }
}
