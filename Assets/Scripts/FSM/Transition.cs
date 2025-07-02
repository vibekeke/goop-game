using GoopGame.Engine;
using UnityEngine;

namespace GoopGame.FSM
{
    /// <summary>
    /// Represents a transition in the finite state machine (FSM) that occurs based on a decision.
    /// </summary>
    public sealed class Transition : ScriptableObject
    {
        public Decision Decision;
        public BaseState OnTrueState;
        public BaseState OnFalseState;

        /// <summary>
        /// Executes the transition based on the decision made by the Decision object.
        /// </summary>
        public void Exectue(Goop goop)
        {
            bool decision = Decision.Decide(goop);

            //If null, it should remain in current state so no state change occurs.
            if (decision && OnTrueState != null)
                goop.SetState(OnTrueState);
            else if (decision && OnFalseState != null)
                goop.SetState(OnFalseState);
        }
    }
}
