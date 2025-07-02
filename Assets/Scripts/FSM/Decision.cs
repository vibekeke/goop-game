using UnityEngine;
using GoopGame.Engine;
using System;
using System.Collections.Generic;

namespace GoopGame.FSM
{
    public abstract class Decision : ScriptableObject
    {
        /// <summary>
        /// Registers a Goop instance with the decision. This is relevant if the decision needs to store data per goop to evaluate.
        /// e.g TimeInState or randomly generated values per instance.
        /// In the inheriting class, store the Goop instance in a dictionary or list for later evaluation.
        /// </summary>
        public virtual void RegisterGoop(Goop goop)
        {
            // Default implementation does nothing
            // Dictionary<Goop, object> .Add(goop);
        }

        /// <summary>
        /// Unregisters a Goop instance from the decision. his is relevant if the decision needs to store data per goop to evaluate.
        /// e.g TimeInState or randomly generated values per instance.
        /// In the inheriting class, release the Goop instance from a dictionary or list used in RegisterGoop.
        /// </summary>
        public virtual void UnregisterGoop(Goop goop)
        {
            // Default implementation does nothing
            // if (Dictionary<Goop, object> .ContainsKey(goop))
            //     Dictionary<Goop, object> .Remove(goop);
        }

        /// <summary>
        /// Indicates whether the decision can be evaluated while the Goop is marked as busy.
        /// </summary>
        public bool CanEvaluateWhileBusy;

        /// <summary>
        /// Decides whether the transition should occur based on the current state of the Goop instance.
        /// </summary>
        /// <returns>If it should transition to true state or false state</returns>
        public abstract bool Decide(Goop goop);
    }
}
