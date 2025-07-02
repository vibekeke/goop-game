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
            State = state;
        }

        private void Update()
        {
            if (State == null)
                return;

            State.Execute(this);
        }
    }
}
