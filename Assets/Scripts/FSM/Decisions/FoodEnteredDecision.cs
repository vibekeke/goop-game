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
    [CreateAssetMenu(fileName = "FoodEnteredDecision", menuName = "GoopGame/FSM/Decisions/Create new FoodEnteredDecision")]
    public class FoodEnteredDecision : Decision
    {
        public override bool Decide(Goop goop)
        {
            return goop.TouchedFood != null;
        }

        public override void UnregisterGoop(Goop goop)
        {
            goop.ClearTouchedFood();
        }
    }
}
