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

        public override void EnterState(Goop goop)
        {
            foreach(var transition in Transitions)
            {
                transition.Decision.RegisterGoop(goop);     // Register the Goop with each decision in the transition
            }
        }

        public override void ExitState(Goop goop)
        {
            foreach (var transition in Transitions)
            {
                transition.Decision.UnregisterGoop(goop);     // Unregister the Goop with each decision in the transition
            }
        }

        /// <summary>
        /// Executes all actions and evaluates all transitions
        /// </summary>
        public override void Execute(Goop goop)
        {
            bool busy = false;
            foreach (var action in States)
            {
                if (action.IsBusyDuringAction)              //check if any action makes goop currently busy.
                    busy = true;
                action.Execute(goop);
            }

            foreach (var transition in Transitions)
            {
                transition.Exectue(goop, busy);
            }
        }
    }
}
