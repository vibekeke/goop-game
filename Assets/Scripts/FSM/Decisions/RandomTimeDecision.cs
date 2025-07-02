using GoopGame.Engine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoopGame.FSM
{
    /// <summary>
    /// A decision that waits for a random amount of time before transitioning to the next state.
    /// Transition should not have a false state.
    /// </summary>
    [CreateAssetMenu(fileName = "RandomTimeDecision", menuName = "GoopGame/FSM/Decisions/Create new RandomTimeDecision")]
    public class RandomTimeDecision : Decision
    {
        [NonSerialized]
        private Dictionary<Goop, float> _goopTimesLeftMap = new Dictionary<Goop, float>();

        /// <summary>
        /// Base time to wait.
        /// </summary>
        public float BaseTime;
        /// <summary>
        /// Potential deviation from the base time.
        /// </summary>
        public float RandomDeviation;

        public override void RegisterGoop(Goop goop)
        {
            _goopTimesLeftMap.Add(goop, BaseTime + UnityEngine.Random.Range(0f, RandomDeviation) - RandomDeviation * 0.5f);
        }

        public override void UnregisterGoop(Goop goop)
        {
            if (_goopTimesLeftMap.ContainsKey(goop))
                _goopTimesLeftMap.Remove(goop);
        }

        public override bool Decide(Goop goop)
        {
            if (!_goopTimesLeftMap.ContainsKey(goop))
            {
                Debug.LogError($"Goop {goop.name} is not registered with {nameof(RandomTimeDecision)}.");
                return false;
            }

            _goopTimesLeftMap[goop] -= Time.deltaTime;

            if (_goopTimesLeftMap[goop] <= 0f)
            {
                return true; // Transition to the next state
            }
            return false;
        }
    }
}
