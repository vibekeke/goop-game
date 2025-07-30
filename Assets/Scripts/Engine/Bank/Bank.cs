using UnityEngine;

namespace GoopGame.Engine
{
    public class Bank
    {

        /// <summary>
        /// constructor for bank object with goop coin variable
        /// <summary>
        public Bank()
        {

            // goop coin variable for game instance set to 100 by default
            int GoopCoins = 100;

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
            // return success value 0 - failure
            if (subtractValue > GoopCoins)
            {
                return 0;
            }

            // remove subtract value from total goop coins
            // return success value 1 - success
            this.GoopCoins -= subtractValue;
            return 1;
            
        }

        /// <summary>
        /// add method for bank instance,
        /// adding set value from goop coin variable
        /// </summary>
        void Add(int addValue)
        {

            this.GoopCoins += addValue;

        }

    }
    

}
