using UnityEngine;

namespace GoopGame.Engine
{
    /// <summary>
    /// Bank class with current goop coins for game instance
    /// To add and subtract from when completing quests and shopping
    /// </summary>
    public class Bank
    {
        // Goop coin variable for bank object, only to be changed within class
        // With initial value 100
        private const int GoopCoinsInitialValue = 100;
        public int GoopCoins { get; private set; }

        /// <summary>
        /// Constructor for new bank object
        /// <summary>
        public Bank()
        {
            // Set goop coin value to initial value on creation
            GoopCoins = GoopCoinsInitialValue;
        }

        /// <summary>
        /// Method for subtracting set value from total goop coins
        /// Returning success value true or false
        /// </summary>
        public bool Subtract(int subtractValue)
        {
            // Return false if it tried to subtract more goop coins than available
            if (subtractValue > GoopCoins)
            {
                return false;
            }

            // Subtract and update desired goop coins from total
            // Returning true on success
            this.GoopCoins -= subtractValue;
            return true;
        }

        /// <summary>
        /// Method for adding set value to total goop coins
        /// </summary>
        public void Add(int addValue)
        {
            this.GoopCoins += addValue;
        }
    } 
}
