using UnityEngine;
using GoopGame.Engine;

namespace GoopGame.FSM
{
    /// <summary>
    /// The base class for actions that can be performed in a state.
    /// </summary>
    public abstract class StateAction : ScriptableObject
    {
        /// <summary>
        /// Indicates whether the action should mark the Goop as busy, relevant to decision evaluations.
        /// </summary>
        public bool IsBusyDuringAction;
        public abstract void Execute(Goop goop);
    }
}
