using GoopGame.Data;
using UnityEngine;

namespace GoopGame.Engine
{
    public class Goop : MonoBehaviour
    {
        [SerializeField]
        private GoopStatData _hungerData, _temperatureData, 
            _moodData, _energyData;
        [SerializeField]
        private GoopTraitDataScalar _sizeData, _speedData;
        [SerializeField]
        private GoopTraitDataColor _colorData;

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

        private bool _initialized = false;

        public void SetState(BaseState state)
        {
            State.ExitState(this); // Exit the current state
            State = state;
            State.EnterState(this); // Enter the new state
        }

        public void Initialize(bool generateNewValues = false)
        {
            Stats = new GoopStats(_hungerData, _temperatureData, 
                _moodData, _energyData);

            if (generateNewValues)
                Traits = new GoopTraits(_sizeData, _speedData,
                    _colorData);

            _initialized = true;

            if (State != null)
                State.EnterState(this);
        }

        private void Start()
        {
            if (_initialized)
                return;

            Initialize(true);
        }

        private void Update()
        {
            if (State == null)
                return;

            State.Execute(this);
        }
    }
}
