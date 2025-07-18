using GoopGame.Data;
using UnityEngine;

namespace GoopGame.Engine
{
    public class Goop : MonoBehaviour
    {
        [SerializeField]
        private GoopStatData _hungerData, _temperatureData, _moodData, _energyData;

        /// <summary>
        /// The mutable stats of a goop instance.
        /// </summary>
        public GoopStats Stats { get; private set; }

        public GoopTraits Traits { get; private set; }

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
            Stats = new GoopStats(_hungerData, _temperatureData, _moodData, _energyData);

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
