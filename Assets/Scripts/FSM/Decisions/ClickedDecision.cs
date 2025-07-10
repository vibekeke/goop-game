using GoopGame;
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
    [CreateAssetMenu(fileName = "ClickedDecision", menuName = "GoopGame/FSM/Decisions/Create new ClickedDecision")]
    public class ClickedDecision : Decision
    {
        public override bool Decide(Goop goop)
        {
            DraggableObject draggable = goop.GetComponent<DraggableObject>();
            return draggable.IsGrabbing;
        }
    }
}
