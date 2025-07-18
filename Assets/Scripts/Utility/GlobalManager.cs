using UnityEngine;

namespace GoopGame.Utility
{
    /// <summary>
    /// A class for storing global values, to limit overhead caused by searching.
    /// </summary>
    public class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// Camera to assign as the main camera, set to Camera.main if null.
        /// </summary>
        [SerializeField]
        private Camera _cameraToSet;
        /// <summary>
        /// The game's current display camera.
        /// </summary>
        public static Camera Camera { get; private set; }

        private void Awake()
        {
            if (_cameraToSet == null)
                Camera = Camera.main;
            else
                Camera = _cameraToSet;
        }
    }
}