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
    [CreateAssetMenu(fileName = "FoodOnScreenDecision", menuName = "GoopGame/FSM/Decisions/Create new FoodOnScreenDecision")]
    public class FoodOnScreenDecision : Decision
    {
        public override bool Decide(Goop goop)
        {
            return false;
        }
    }
}
