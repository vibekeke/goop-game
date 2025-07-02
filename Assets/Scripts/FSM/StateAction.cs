using UnityEngine;
using GoopGame.Engine;

namespace GoopGame.FSM
{
    /// <summary>
    /// The base class for actions that can be performed in a state.
    /// </summary>
    public abstract class StateAction : ScriptableObject
    {
        public abstract void Execute(Goop goop);
    }
}
