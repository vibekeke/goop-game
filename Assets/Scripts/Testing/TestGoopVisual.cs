using GoopGame.Engine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoopGame.Testing
{
    public class TestGoopVisual : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private TMP_Text _text;

        public Button RemoveButton, SplitButton, CombineWithNextButton;

        [SerializeField]
        private Goop _goop;

        private void Reset()
        {
            _goop = GetComponent<Goop>();
            _image = GetComponent<Image>();
        }

        public void SetVisual()
        {
            _image.color = _goop.Traits.Color.Value;
            transform.localScale = Vector3.one * _goop.Traits.Size.Value / 5f;

            _text.text = $"Size: {_goop.Traits.Size.Value}\n" +
                $"Speed: {_goop.Traits.Speed.Value}";
        }
    }
}
