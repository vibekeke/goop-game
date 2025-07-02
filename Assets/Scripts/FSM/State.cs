using GoopGame.Engine;
using System.Collections.Generic;
using UnityEngine;

namespace GoopGame.FSM
{
    /// <summary>
    /// A state in a finite state machine (FSM).
    /// </summary>
    [CreateAssetMenu(fileName = "State", menuName = "GoopGame/FSM/Create new State", order = 0)]
    public sealed class State : BaseState
    {
        /// <summary>
        /// List of actions to execute when the state is active.
        /// </summary>
        public List<StateAction> States;
        /// <summary>
        /// List of transitions to evaluate when the state is active.
        /// </summary>
        public List<Transition> Transitions;

        /// <summary>
        /// Executes all actions and evaluates all transitions
        /// </summary>
        public override void Execute(Goop goop)
        {
            foreach (var action in States)
                action.Execute(goop);

            foreach (var transition in Transitions)
                transition.Exectue(goop);
        }
    }
}
