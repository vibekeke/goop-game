using GoopGame.Data;
using UnityEngine;

namespace GoopGame.Engine
{
    /// <summary>
    /// A class containing the value of a stat and reference to its immutable data.
    /// Responsible for clamping the value, and parsing its current state.
    /// </summary>
    public class GoopStat
    {
        public GoopStat(GoopStatData data)
        {
            _data = data;
            _value = data.DefaultValue;
        }

        /// <summary>
        /// The immutable data is set to private, and its values are not accessible out of scope.
        /// This is to avoid any accidental writes to the ScriptableObject.
        /// </summary>
        private GoopStatData _data;

        private float _value;
        public float Value
        {
            get { return _value; }
            set
            {
                _value = Mathf.Clamp(value, _data.MinimumValue, _data.MaximumValue);
            }
        }

        /// <summary>
        /// The current state that corresponds with the thresholds defined in GoopStatData.
        /// Returns an int, which is then converted to the appropriate enum.
        /// </summary>
        public int State
        {
            get
            {
                if (Value < _data.LessThanThreshold)
                    return 1;
                if (Value > _data.GreaterThanThreshold)
                    return 2;
                return 3;
            }
        }
    }
}
