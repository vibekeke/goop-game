using UnityEngine;

namespace GoopGame.Engine
{
    public class Goop : MonoBehaviour
    {
        /// <summary>
        /// Current FSM state.
        /// </summary>
        public BaseState State { get; private set; }

        public void SetState(BaseState state)
        {
            State = state;
        }
    }
}
