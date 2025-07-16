using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// Immutable data related to a singular goop stat.
    /// Defines the constraints of the stat, and the thresholds that defines the stat's different states.
    /// </summary>
    [CreateAssetMenu(fileName = "GoopStatData", menuName = "GoopGame/Goops/Create new GoopStatData")]
    public class GoopStatData : ScriptableObject
    {
        public GoopStatTypes StatType;
        public float DefaultValue;
        public float MinimumValue;
        public float MaximumValue;
        public float LessThanThreshold;
        public float GreaterThanThreshold;
    }
}
