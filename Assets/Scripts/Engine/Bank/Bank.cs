using UnityEngine;

namespace GoopGame.Engine
{
    /// <summary>
    /// bank class with current goop coins for game instance
    /// to add and subtract from when completing quests and shopping
    /// </summary>
    public class Bank
    {
        // goop coin variable for bank object
        private int GoopCoins;

        /// <summary>
        /// constructor for new bank object
        /// <summary>
        public Bank()
        {
            // goop coin variable for game instance set to 100 by default
            GoopCoins = 100;
        }

        /// <summary>
        /// subtract method for bank instance,
        /// removing set value from goop coin variable
        /// returning success value 0 or 1
        /// </summary>
        bool Subtract(int subtractValue)
        {
            // if subtract value is too much,
            // coins cant be subtracted from total goop coins
            // return success value false - failure
            if (subtractValue > GoopCoins)
            {
                return false;
            }

            // remove subtract value from total goop coins
            // return success value true - success
            this.GoopCoins -= subtractValue;
            return true;
        }

        /// <summary>
        /// add method for bank instance,
        /// adding set value from goop coin variable
        /// </summary>
        void Add(int addValue)
        {
            this.GoopCoins += addValue;
        }

        int GetGoopCoins()
        {
            return this.GoopCoins;
        }
    } 
}
