using GoopGame.Data;

namespace GoopGame.Engine
{
    /// <summary>
    /// A class containing the mutable stats per Goop instance.
    /// Any change to these values should be done through the FSM implementation.
    /// </summary>
    public class GoopStats
    {
        /// <summary>
        /// Generates a new GoopStats instance. Requires references to immutable
        /// data related to each unique stat.
        /// </summary>
        public GoopStats(
            GoopStatData hunger,GoopStatData temperature,
            GoopStatData mood, GoopStatData energy
            )
        {
            _hunger = new GoopStat(hunger);
            _temperature = new GoopStat(temperature);
            _mood = new GoopStat(mood);
            _energy = new GoopStat(energy);
            Age = 0f;
        }

        public float Age;

        /// <summary>
        /// The stat objects are set to private, as the only elements relevant
        /// outside of scope are their state and value fields.
        /// </summary>
        private GoopStat _hunger, _temperature, _mood, _energy;
        public float Hunger
        {
            get { return _hunger.Value; }
            set { _hunger.Value = value; }
        }
        public float Temperature
        {
            get { return _temperature.Value; }
            set { _temperature.Value = value; }
        }
        public float Mood
        {
            get { return _mood.Value; }
            set { _mood.Value = value; }
        }
        public float Energy
        {
            get { return _energy.Value; }
            set { _energy.Value = value; }
        }

        public HungerState HungerState => (HungerState)_hunger.State;
        public TemperatureState TemperatureState => (TemperatureState)_temperature.State;
        public MoodState MoodState => (MoodState)_mood.State;
        public EnergyState EnergyState => (EnergyState)_energy.State;
    }
}
