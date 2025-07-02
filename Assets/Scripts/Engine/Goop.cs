using UnityEngine;

namespace GoopGame.Engine
{
    public class Goop : MonoBehaviour
    {
        /// <summary>
        /// Current FSM state.
        /// </summary>
        [field: SerializeField]
        public BaseState State { get; private set; }

        public void SetState(BaseState state)
        {
            State.ExitState(this); // Exit the current state
            State = state;
            State.EnterState(this); // Enter the new state
        }

        private void Awake()
        {
            if (State != null)
                State.EnterState(this);
        }

        private void Update()
        {
            if (State == null)
                return;

            State.Execute(this);
        }
    }
}
