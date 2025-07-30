using UnityEngine;

namespace GoopGame.Engine
{
    public class GlobalManager : MonoBehaviour
    {
        public static GlobalManager Instance; // static, permanent global manager instance for game

        // uninitialized bank instance for game
        public Bank Bank;

        private void Awake()
        {
            // check if Instance is set (not null), and that Instance isn't set to this instance
            if (Instance != null && Instance != this)
            {
                // if it already exists, destroy this GlobalManager instance, since it already exists
                Destroy(gameObject);
                return;
            }
            // if we have not exited and GlobalManager is uninitialized, set Instance to this
            Instance = this;
            Bank = new Bank();
        }
        
        public void Foo()
        {
            Debug.Log($"Goop coins in bank: {GlobalManager.Instance.Bank.GetGoopCoins()}");
        }
    }
}
