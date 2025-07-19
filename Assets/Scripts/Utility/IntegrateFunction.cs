using System;
using UnityEngine;

namespace GoopGame.Utility
{
    //Created using "Unity Animation Curves for Sampling" by Sebastian Schöner
    //retrieved from https://blog.s-schoener.com/2018-05-05-animation-curves/
    
    //Additional comments added for more context.

    /// <summary>
    /// Creates an approximation of a functions definite integral.
    /// </summary>
    public class IntegrateFunction
    {
        private Func<float, float> _function;
        private float[] _values;
        private float _from, _to;

        /// <summary>
        /// Integrates a function on an interval. Use the steps parameter to control
        /// the precision of the numerical integration. Larger step values lead to
        /// better precision.
        /// </summary>
        public IntegrateFunction(Func<float, float> func, float from, float to, int steps)
        {
            _values = new float[steps + 1];
            //the function to integrate
            _function = func;
            //start x value, usually 0
            _from = from;
            //end x value, usually 1
            _to = to;
            ComputeValues();
        }

        /// <summary>
        /// Numerical integration trapezoid rule
        /// </summary>
        private void ComputeValues()
        {
            int n = _values.Length;
            //divide function into segments based on steps
            float segment = (_to - _from) / (n - 1);
            //track last y value to form each segment's trapezoid
            float lastY = _function(_from);
            float sum = 0;
            _values[0] = 0;
            for (int i = 1; i < n; i++)
            {
                //find start of segment
                float x = _from + i * segment;
                //find next y value to form segment's trapezoid
                float nextY = _function(x);
                //calculate area of trapezoid and add to sum
                sum += segment * (nextY + lastY) / 2;
                //update last y value
                lastY = nextY;
                //set value to area thus far.
                _values[i] = sum;
            }
        }

        /// <summary>
        /// Evaluates the integrated function at any point in the interval.
        /// </summary>
        public float Evaluate(float x)
        {
            Debug.Assert(_from <= x && x <= _to);
            //convert point to 0,1 range
            float t = Mathf.InverseLerp(_from, _to, x);
            //find corresponding lower approximation by converting point to index in array
            int lower = (int)(t * _values.Length);
            //find upper approximation, truncating to same value if within .5 of the segments starting point
            int upper = (int)(t * _values.Length + .5f);
            //if upper value was truncated (or out of bounds), the lower trapezoid is accurate enough
            if (lower == upper || upper >= _values.Length)
                return _values[lower];
            //if not, find corresponding value by lerping between the two values
            float innerT = Mathf.InverseLerp(lower, upper, t * _values.Length);
            return (1 - innerT) * _values[lower] + innerT * _values[upper];
        }

        /// <summary>
        /// Returns the total value integrated over the whole interval.
        /// </summary>
        public float Total
        {
            get
            {
                return _values[_values.Length - 1];
            }
        }
    }
}
