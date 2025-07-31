using UnityEngine;

namespace GoopGame.Utility
{
    /// <summary>
    /// A class for storing global values, to limit overhead caused by searching.
    /// </summary>
    public class GlobalManager : MonoBehaviour
    {
        // Static, permanent global manager instance for game
        public static GlobalManager Instance;
        
        /// <summary>
        /// Camera to assign as the main camera, set to Camera.main if null.
        /// </summary>
        [SerializeField]
        private Camera _cameraToSet;
        /// <summary>
        /// The game's current display camera.
        /// </summary>
        public static Camera Camera { get; private set; }

        // Uninitialized bank instance for game
        public Bank Bank;

        private void Awake()
        {
            // Check if Instance is set (not null), and that Instance isn't set to this instance
            if (Instance != null && Instance != this)
            {
            // If it already exists, destroy this GlobalManager instance, since it already exists
                Destroy(gameObject);
                return;
            }
            // If we have not exited and GlobalManager is uninitialized, set Instance to this
            // And set bank object to a new one
            Instance = this;
            Bank = new Bank();

            if (_cameraToSet == null)
                Camera = Camera.main;
            else
                Camera = _cameraToSet;
        }
    }
}