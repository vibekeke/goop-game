using GoopGame.Engine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoopGame.Testing
{
    /// <summary>
    /// A simple UI representation of a goop's size, color and speed traits.
    /// </summary>
    public class TestGoopVisual : MonoBehaviour
    {
        /// <summary>
        /// Image to set to a goop's color.
        /// </summary>
        [SerializeField]
        private Image _image;

        /// <summary>
        /// Text field for debugging info.
        /// </summary>
        [SerializeField]
        private TMP_Text _text;

        /// <summary>
        /// Buttons to combine, split and remove goops.
        /// </summary>
        public Button RemoveButton, SplitButton, CombineWithNextButton;

        /// <summary>
        /// Reference to the actual goop class.
        /// </summary>
        [SerializeField]
        private Goop _goop;

        private void Reset()
        {
            _goop = GetComponent<Goop>();
            _image = GetComponent<Image>();
        }

        /// <summary>
        /// Set visuals to match goop data.
        /// </summary>
        public void SetVisual()
        {
            _image.color = _goop.Traits.Color.Value;
            transform.localScale = Vector3.one * _goop.Traits.Size.Value / 5f;

            _text.text = $"Size: {_goop.Traits.Size.Value}\n" +
                $"Speed: {_goop.Traits.Speed.Value}";
        }
    }
}
