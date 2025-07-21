using GoopGame.Data;
using UnityEngine;

namespace GoopGame.Engine
{
    public class Goop : MonoBehaviour
    {
        /// <summary>
        /// Static stat data.
        /// </summary>
        [SerializeField]
        private GoopStatData _hungerData, _temperatureData, 
            _moodData, _energyData;

        /// <summary>
        /// Static float trait data.
        /// </summary>
        [SerializeField]
        private GoopTraitDataScalar _sizeData, _speedData;

        /// <summary>
        /// Static color trait data.
        /// </summary>
        [SerializeField]
        private GoopTraitDataColor _colorData;

        /// <summary>
        /// The mutable stats of a goop instance.
        /// </summary>
        public GoopStats Stats { get; private set; }

        /// <summary>
        /// The immutable stats of a goop instance.
        /// </summary>
        public GoopTraits Traits { get; private set; }

        /// <summary>
        /// Current FSM state.
        /// </summary>
        [field: SerializeField]
        public BaseState State { get; private set; }

        /// <summary>
        /// Determines if the goop initializes itself on Start()
        /// </summary>
        private bool _initialized = false;

        /// <summary>
        /// Sets FSM state of goop.
        /// </summary>
        public void SetState(BaseState state)
        {
            State.ExitState(this); // Exit the current state
            State = state;
            State.EnterState(this); // Enter the new state
        }

        /// <summary>
        /// Sets the traits of this goop. Should be called before
        /// initializing goop to avoid generating new traits.
        /// </summary>
        public void SetTraits(GoopTraits traits)
        {
            Traits = traits;
        }

        /// <summary>
        /// Initializes the goop, and generates random traits if no traits
        /// exist.
        /// </summary>
        public void Initialize()
        {
            Stats = new GoopStats(_hungerData, _temperatureData, 
                _moodData, _energyData);

            if (Traits == null)
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

            Initialize();
        }

        private void Update()
        {
            if (State == null)
                return;

            State.Execute(this);
        }
    }
}
