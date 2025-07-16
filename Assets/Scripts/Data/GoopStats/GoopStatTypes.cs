namespace GoopGame.Data
{
    /// <summary>
    /// The different stats a goop tracks.
    /// Utilizes bit fields in case an action/event references multiple stats.
    /// </summary>
    public enum GoopStatTypes
    {
        None = 0,
        Hunger = 1 << 0,
        Temperature = 1 << 1,
        Mood = 1 << 2,
        Energy = 1 << 3,
        Age = 1 << 4
    }
}
