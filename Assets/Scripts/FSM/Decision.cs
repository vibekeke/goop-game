using UnityEngine;
using GoopGame.Engine;

namespace GoopGame.FSM
{
    public abstract class Decision : ScriptableObject
    {
        /// <summary>
        /// Decides whether the transition should occur based on the current state of the Goop instance.
        /// </summary>
        /// <returns>If it should transition to true state or false state</returns>
        public abstract bool Decide(Goop goop);
    }
}
