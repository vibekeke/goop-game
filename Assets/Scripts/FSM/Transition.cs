using GoopGame.Engine;
using System;
using UnityEngine;

namespace GoopGame.FSM
{
    /// <summary>
    /// Represents a transition in the finite state machine (FSM) that occurs based on a decision.
    /// Responsible for determining the next state based on the outcome of a decision.
    /// </summary>
    [Serializable]
    public sealed class Transition
    {
        public Decision Decision;
        public BaseState OnTrueState;
        public BaseState OnFalseState;

        /// <summary>
        /// Executes the transition based on the decision made by the Decision object.
        /// </summary>
        /// <param name="busy"> Indicates whether the Goop is currently busy.</param>
        public void Exectue(Goop goop, bool busy)
        {
            // If the decision cannot be evaluated while busy and Goop is busy, skip evaluation.
            if (!Decision.CanEvaluateWhileBusy && busy)
                return;

            bool decision = Decision.Decide(goop);

            //If null, it should remain in current state so no state change occurs.
            if (decision && OnTrueState != null)
                goop.SetState(OnTrueState);
            else if (decision && OnFalseState != null)
                goop.SetState(OnFalseState);

            
            Debug.Log($"[FSM] Checking “{Decision.name}” → OnTrueState = {OnTrueState?.name ?? "NULL"}");
            Debug.Log($"[FSM] Decision “{Decision.name}” returned {decision}");
            if (decision && OnTrueState != null)
            {
                Debug.Log($"[FSM] → Transition firing, going to {OnTrueState.name}");
                goop.SetState(OnTrueState);
            }
            else if (decision && OnFalseState != null)
            {
                Debug.Log($"[FSM] → Transition firing (false path), going to {OnFalseState.name}");
                goop.SetState(OnFalseState);
            }

        }
    }
}
